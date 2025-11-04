using MediatR;
using lexicana.Database;
using lexicana.Endpoints;
using Microsoft.EntityFrameworkCore;
using lexicana.TopicFolder.WordFolder.DTOs;

namespace lexicana.TopicFolder.Queries.GetTopicWords;

public record GetTopicWordsRequest(Guid Id) : IHttpRequest<List<WordModel>>;

public class Handler: IRequestHandler<GetTopicWordsRequest, Response<List<WordModel>>>
{
    private readonly ApplicationDbContext _context;

    public Handler(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Response<List<WordModel>>> Handle(GetTopicWordsRequest request, CancellationToken cancellationToken)
    {
        var topicExists = await _context.Topics.AnyAsync(t => t.Id == request.Id, cancellationToken);
        if (!topicExists)
            return FailureResponses.NotFound<List<WordModel>>("Topic not found");
        
        var words = await _context.TopicWords
            .Where(tw => tw.TopicId == request.Id)
            .Include(tw => tw.Word)
            .OrderBy(tw => tw.Order)
            .Select(tw => tw.Word)
            .ToListAsync(cancellationToken);
        
        var result = words.Select(w => new WordModel
        {
            Id = w.Id,
            Word = w.Value,
            Translation = w.Translation
        }).ToList();
        
        return SuccessResponses.Ok(result);
    }
}