using MediatR;
using lexicana.Database;
using lexicana.Endpoints;
using Microsoft.EntityFrameworkCore;
using lexicana.Authorization.Services;
using lexicana.UserFolder.UserLessonFolder.Entities;
using lexicana.UserFolder.UserLessonFolder.Enums;

namespace lexicana.UserFolder.UserLessonFolder.Command.CompleteLesson;

public record CompleteUserLessonRequest(Guid Id) : IHttpRequest<EmptyValue>;

public class Handler: IRequestHandler<CompleteUserLessonRequest, Response<EmptyValue>>
{
    private readonly AuthService _authService;
    private readonly ApplicationDbContext _context;

    public Handler(AuthService authService, ApplicationDbContext context)
    {
        _context = context;
        _authService = authService;
    }
    
    public async Task<Response<EmptyValue>> Handle(CompleteUserLessonRequest request, CancellationToken cancellationToken)
    {
        var userId = _authService.GetCurrentUserId();
        
        var userLesson = await _context.UserLessons
            .Include(x=>x.Lesson)
            .FirstOrDefaultAsync(x =>
                x.LessonId == request.Id
                && x.UserId == userId
            );

        if (userLesson is null)
            return FailureResponses.NotFound("User lesson not found");
        
        userLesson.CompleatedWordsIds.Clear();

        if (userLesson.Status == UserLessonStatus.Old)
        {
            await _context.SaveChangesAsync();
            return SuccessResponses.Ok();
        }
        
        userLesson.Status = UserLessonStatus.Old;
        
        var language = userLesson.Lesson.Language;

        var nextLesson = await _context.Lessons
            .Where(t => t.Language == language && t.Order > userLesson.Lesson.Order)
            .OrderBy(t => t.Order)
            .FirstOrDefaultAsync();

        if (nextLesson is null) return SuccessResponses.Ok();   

        await _context.UserLessons.AddAsync(new UserLesson()
        {
            LessonId = nextLesson.Id,
            UserId = userLesson.UserId,
            Status = UserLessonStatus.Current
        });
        
        await _context.SaveChangesAsync();
        return SuccessResponses.Ok();
    }
}