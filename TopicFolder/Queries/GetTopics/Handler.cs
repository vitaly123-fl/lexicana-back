using MediatR;
using lexicana.Database;
using lexicana.Endpoints;
using Microsoft.EntityFrameworkCore;
using lexicana.Authorization.Services;

namespace lexicana.TopicFolder.Queries.GetTopics;

public record GetTopicsRequest() : IHttpRequest<List<TopicsResponseBody>>;

public record TopicsResponseBody(
    Guid Id,
    string Title,
    bool IsGrouped,
    bool IsPremium
);

public class Handler: IRequestHandler<GetTopicsRequest, Response<List<TopicsResponseBody>>>
{
    private readonly AuthService _authService;
    private readonly ApplicationDbContext _context;

    public Handler(ApplicationDbContext context, AuthService authService)
    {
        _context = context;
        _authService = authService;
    }
    
    public async Task<Response<List<TopicsResponseBody>>> Handle(GetTopicsRequest request, CancellationToken cancellationToken)
    {
        var userId = _authService.GetCurrentUserId();
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

        if (user is null)
            return FailureResponses.NotFound<List<TopicsResponseBody>>("User not found");

        var topics = await _context.Topics
            .Where(t => t.Language == user.Language)
            .OrderBy(t => t.Order)
            .Select(t => new TopicsResponseBody(
                t.Id,
                t.Title,
                t.IsGrouped,
                t.IsPremium
            ))
            .ToListAsync(cancellationToken);
        
        return SuccessResponses.Ok(topics);
    }
}