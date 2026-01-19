using MediatR;
using lexicana.Database;
using lexicana.Endpoints;
using lexicana.LessonFolder.TopicFolder.WordFolder.DTOs;
using Microsoft.EntityFrameworkCore;

namespace lexicana.LessonFolder.Queries.GetLessonWords;

public record GetLessonWordsRequest(Guid Id) : IHttpRequest<List<WordModel>>;

public class Handler : IRequestHandler<GetLessonWordsRequest, Response<List<WordModel>>>
{
    private readonly ApplicationDbContext _context;

    public Handler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Response<List<WordModel>>> Handle(GetLessonWordsRequest request, CancellationToken cancellationToken)
    {
        var lessonExists = await _context.Lessons.AnyAsync(l => l.Id == request.Id, cancellationToken);
        
        if (!lessonExists)
            return FailureResponses.NotFound<List<WordModel>>("Lesson not found");

        var words = await _context.Lessons
            .Where(l => l.Id == request.Id)
            .SelectMany(l => l.Topics.OrderBy(t => t.CreateAt))
            .SelectMany(t => t.Words.OrderBy(w => w.CreateAt))
            .Select(w => new WordModel
            {
                Id = w.Id,
                Word = w.Value,
                Translation = w.Translation
            })
            .ToListAsync(cancellationToken);

        return SuccessResponses.Ok(words);
    }
}