using MediatR;
using lexicana.Database;
using lexicana.Endpoints;
using Microsoft.EntityFrameworkCore;
using lexicana.Authorization.Services;

namespace lexicana.LessonFolder.Queries.GetLessons;

public record GetLessonsRequest() : IHttpRequest<List<LessonsResponseBody>>;

public record LessonsResponseBody(
    Guid Id,
    string Title,
    bool IsGrouped,
    bool IsPremium
);

public class Handler: IRequestHandler<GetLessonsRequest, Response<List<LessonsResponseBody>>>
{
    private readonly AuthService _authService;
    private readonly ApplicationDbContext _context;

    public Handler(ApplicationDbContext context, AuthService authService)
    {
        _context = context;
        _authService = authService;
    }
    
    public async Task<Response<List<LessonsResponseBody>>> Handle(GetLessonsRequest request, CancellationToken cancellationToken)
    {
        var userId = _authService.GetCurrentUserId();
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

        if (user is null)
            return FailureResponses.NotFound<List<LessonsResponseBody>>("User not found");

        var lessons = await _context.Lessons
            .Where(t => t.Language == user.Language)
            .OrderBy(t => t.Order)
            .Select(t => new LessonsResponseBody(
                t.Id,
                t.Title,
                t.Topics.Count > 1,
                t.IsPremium
            ))
            .ToListAsync(cancellationToken);
        
        return SuccessResponses.Ok(lessons);
    }
}