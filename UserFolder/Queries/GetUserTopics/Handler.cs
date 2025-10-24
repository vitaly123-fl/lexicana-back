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
        var user = await _context.Users
            .Include(x => x.UserTopics)
            .ThenInclude(x=>x.Topic)
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

        if (user is null)
            return FailureResponses.NotFound<List<UserTopicsResponseBody>>("User not found");

        var userTopics = user.UserTopics
            .Where(x => x.Topic.Language == user.Language)
            .Select(x => new UserTopicsResponseBody(
                x.TopicId,
                x.Status
            )).ToList();
        
        return SuccessResponses.Ok(userTopics);
    }
}