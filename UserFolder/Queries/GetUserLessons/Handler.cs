using MediatR;
using lexicana.Database;
using lexicana.Endpoints;
using Microsoft.EntityFrameworkCore;
using lexicana.Authorization.Services;
using lexicana.UserFolder.UserLessonFolder.Enums;

namespace lexicana.UserFolder.Queries.GetUserLessons;

public record GetUserLessonsRequest() : IHttpRequest<List<UserLessonsResponseBody>>;

public record UserLessonsResponseBody(
    Guid LessonId,
    UserLessonStatus Status
);

public class Handler: IRequestHandler<GetUserLessonsRequest, Response<List<UserLessonsResponseBody>>>
{
    private readonly AuthService _authService;
    private readonly ApplicationDbContext _context;

    public Handler(ApplicationDbContext context, AuthService authService)
    {
        _context = context;
        _authService = authService;
    }
    
    public async Task<Response<List<UserLessonsResponseBody>>> Handle(GetUserLessonsRequest request, CancellationToken cancellationToken)
    {
        var userId = _authService.GetCurrentUserId();
        var userLanguage = await _context.Users
            .Where(u => u.Id == userId)
            .Select(u => u.Language)
            .FirstOrDefaultAsync(cancellationToken);

        if (userLanguage is null)
            return FailureResponses.NotFound<List<UserLessonsResponseBody>>("User not found");

        var userLessons = await _context.UserLessons
            .Where(ut => ut.UserId == userId && ut.Lesson.Language == userLanguage)
            .Select(ut => new UserLessonsResponseBody(
                ut.LessonId,
                ut.Status
            ))
            .ToListAsync(cancellationToken);

        
        return SuccessResponses.Ok(userLessons);
    }
}