using MediatR;
using CsvHelper;
using System.Text;
using lexicana.Database;
using lexicana.Endpoints;
using System.Globalization;
using lexicana.Common.Enums;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using lexicana.TopicFolder.Entities;
using CsvHelper.Configuration.Attributes;
using lexicana.TopicFolder.WordFolder.Entities;
using lexicana.TopicFolder.TopicWordFolder.Entities;

namespace lexicana.ParserFolder.Commands.TopicWordParser;

public record WordParserCommand(IFormFile File, Language Language) : IHttpRequest<List<WordRecord>>;

public class WordRecord
{
    public string Word { get; set; }
    [Name("principal word translation")]
    public string PrincipalWordTranslation { get; set; }
}

public class Handler: IRequestHandler<WordParserCommand, Response<List<WordRecord>>>
{
    private readonly ApplicationDbContext _context;

    public Handler(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Response<List<WordRecord>>> Handle(WordParserCommand request, CancellationToken cancellationToken)
    {
        var file = request.File;
        
        if (file.Length == 0)
            return FailureResponses.BadRequest<List<WordRecord>>("Bad file");
        
        using var reader = new StreamReader(file.OpenReadStream(), Encoding.UTF8);
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HeaderValidated = null,
            MissingFieldFound = null,
            PrepareHeaderForMatch = args => args.Header.ToLower().Trim()
        };
        
        using var csv = new CsvReader(reader, config);
        var records = csv.GetRecords<WordRecord>().ToList();
        
        if (!records.Any())
            return FailureResponses.BadRequest<List<WordRecord>>("No records found");
        
        var lastOrder = await _context.Topics
            .Where(t => t.Language == request.Language)
            .OrderByDescending(t => t.Order)
            .Select(t => t.Order)
            .FirstOrDefaultAsync(cancellationToken);
        
        var batches = records
            .Select((r, i) => new { Record = r, Index = i })
            .GroupBy(x => x.Index / 100)
            .Select(g => g.Select(x => x.Record).ToList())
            .ToList();
        
        var createdTopics = new List<Topic>();
        
        foreach (var batch in batches)
        {
            lastOrder++;
            
            var topic = new Topic
            {
                IsPremium = true,
                Title = $"Topic {lastOrder}",
                Order = lastOrder,
                Language = request.Language
            };

            int wordOrder = 0;
            foreach (var record in batch)
            {
                var existingWord = await _context.Words.FirstOrDefaultAsync(w =>
                    w.Value.ToLower() == record.Word.ToLower() &&
                    w.Language == request.Language
                );

                Word wordEntity = existingWord ?? new Word
                {
                    Id = Guid.NewGuid(),
                    Value = record.Word,
                    Translation = record.PrincipalWordTranslation,
                    Language = request.Language
                };

                if (existingWord == null)
                    _context.Words.Add(wordEntity);

                var topicWord = new TopicWord
                {
                    Id = Guid.NewGuid(),
                    Topic = topic,
                    Word = wordEntity,
                    Order = wordOrder++
                };

                topic.Words.Add(topicWord);
            }

            _context.Topics.Add(topic);
            createdTopics.Add(topic);
        }
        
        await _context.SaveChangesAsync();
        return SuccessResponses.Ok(records);
    }
}