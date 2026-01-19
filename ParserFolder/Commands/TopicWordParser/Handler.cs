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
using lexicana.LessonFolder.Entites;
using lexicana.TopicFolder.WordFolder.Entities;

namespace lexicana.ParserFolder.Commands.TopicWordParser;

public record WordParserCommand(IFormFile File, Language Language) : IHttpRequest<List<WordRecord>>;

public class WordRecord
{
    public string Word { get; set; }
    [Name("principal word translation")]
    public string PrincipalWordTranslation { get; set; }
}

public class Handler : IRequestHandler<WordParserCommand, Response<List<WordRecord>>>
{
    private readonly ApplicationDbContext _context;
    
    private const int WordsPerTopic = 20;
    private const int TopicsPerReviewLesson = 5;

    public Handler(ApplicationDbContext context) => _context = context;

    public async Task<Response<List<WordRecord>>> Handle(WordParserCommand request, CancellationToken cancellationToken)
    {
        if (request.File.Length == 0)
            return FailureResponses.BadRequest<List<WordRecord>>("Bad file");

        var records = ParseCsv(request.File);
        
        if (records.Count == 0)
            return FailureResponses.BadRequest<List<WordRecord>>("No records found");

        var maxLessonOrder = await _context.Lessons
            .Where(l => l.Language == request.Language)
            .MaxAsync(l => (int?)l.Order, cancellationToken) ?? 0;

        var existingTopicsCount = await _context.Topics
            .CountAsync(t => t.Language == request.Language, cancellationToken);

        var recentTopics = await _context.Topics
            .Where(t => t.Language == request.Language)
            .OrderByDescending(t => t.CreateAt)
            .Take(TopicsPerReviewLesson - 1)
            .OrderBy(t => t.CreateAt)
            .ToListAsync(cancellationToken);

        var newTopics = new List<Topic>();
        
        var batches = records.Chunk(WordsPerTopic);

        foreach (var batch in batches)
        {
            var topic = CreateTopic(request.Language, batch);
            newTopics.Add(topic);
            existingTopicsCount++;

            var lesson = CreateLesson(request.Language, ++maxLessonOrder, existingTopicsCount, topic);
            _context.Lessons.Add(lesson);

            if (existingTopicsCount % TopicsPerReviewLesson != 0) continue;

            var topicsForReview = recentTopics
                .Concat(newTopics)
                .TakeLast(TopicsPerReviewLesson)
                .ToList();

            var reviewLesson = CreateReviewLesson(request.Language, ++maxLessonOrder, existingTopicsCount, topicsForReview);
            _context.Lessons.Add(reviewLesson);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return SuccessResponses.Ok(records);
    }

    private static List<WordRecord> ParseCsv(IFormFile file)
    {
        using var reader = new StreamReader(file.OpenReadStream(), Encoding.UTF8);
        
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HeaderValidated = null,
            MissingFieldFound = null,
            PrepareHeaderForMatch = args => args.Header.ToLower().Trim()
        };

        using var csv = new CsvReader(reader, config);
        return csv.GetRecords<WordRecord>().ToList();
    }

    private Topic CreateTopic(Language language, IEnumerable<WordRecord> records)
    {
        var topic = new Topic { Language = language };

        foreach (var record in records)
        {
            var word = new Word
            {
                Value = record.Word,
                Translation = record.PrincipalWordTranslation,
                Language = language,
                TopicId = topic.Id,
                Topic = topic
            };

            topic.Words.Add(word);
        }

        _context.Topics.Add(topic);
        return topic;
    }

    private static Lesson CreateLesson(Language language, int order, int topicNumber, Topic topic)
    {
        var (start, end) = GetWordRange(topicNumber, 1);
        
        return new Lesson
        {
            Order = order,
            Title = $"{start}-{end}",
            IsPremium = true,
            Language = language,
            Topics = [topic]
        };
    }

    private static Lesson CreateReviewLesson(Language language, int order, int topicNumber, List<Topic> topics)
    {
        var (start, end) = GetWordRange(topicNumber, TopicsPerReviewLesson);
        
        return new Lesson
        {
            Order = order,
            Title = $"{start}-{end}",
            IsPremium = true,
            Language = language,
            Topics = topics
        };
    }

    private static (int start, int end) GetWordRange(int topicNumber, int topicsCount)
    {
        var end = topicNumber * WordsPerTopic;
        var start = end - (topicsCount * WordsPerTopic) + 1;
        return (start, end);
    }
}