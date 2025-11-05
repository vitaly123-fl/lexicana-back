using MediatR;
using lexicana.Database;
using lexicana.Endpoints;
using Microsoft.EntityFrameworkCore;
using lexicana.Authorization.Services;
using lexicana.UserFolder.UserTopicFolder.Enums;

namespace lexicana.UserFolder.Queries.GetUserTopics;

public record GetUserTopicsRequest() : IHttpRequest<List<UserTopicsResponseBody>>;

public record UserTopicsResponseBody(
    Guid TopicId,
    UserTopicStatus Status
);

public class Handler: IRequestHandler<GetUserTopicsRequest, Response<List<UserTopicsResponseBody>>>
{
    private readonly AuthService _authService;
    private readonly ApplicationDbContext _context;

    public Handler(ApplicationDbContext context, AuthService authService)
    {
        _context = context;
        _authService = authService;
    }
    
    public async Task<Response<List<UserTopicsResponseBody>>> Handle(GetUserTopicsRequest request, CancellationToken cancellationToken)
    {
        var userId = _authService.GetCurrentUserId();
        var userLanguage = await _context.Users
            .Where(u => u.Id == userId)
            .Select(u => u.Language)
            .FirstOrDefaultAsync(cancellationToken);

        if (userLanguage is null)
            return FailureResponses.NotFound<List<UserTopicsResponseBody>>("User not found");

        var userTopics = await _context.UserTopics
            .Where(ut => ut.UserId == userId && ut.Topic.Language == userLanguage)
            .Select(ut => new UserTopicsResponseBody(
                ut.TopicId,
                ut.Status
            ))
            .ToListAsync(cancellationToken);

        
        return SuccessResponses.Ok(userTopics);
    }
}