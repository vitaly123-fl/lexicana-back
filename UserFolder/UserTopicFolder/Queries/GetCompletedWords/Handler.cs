using lexicana.Authorization.Services;
using lexicana.Database;
using lexicana.Endpoints;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace lexicana.UserFolder.UserTopicFolder.Queries.GetCompletedWords;

public record GetCompletedWordsRequest(Guid Id): IHttpRequest<List<Guid>>;

public class Handler: IRequestHandler<GetCompletedWordsRequest, Response<List<Guid>>>
{
    private readonly AuthService _authService;
    private readonly ApplicationDbContext _context;
    
    public Handler(ApplicationDbContext context, AuthService authService)
    {
        _context = context;
        _authService = authService;
    }
    
    public async Task<Response<List<Guid>>> Handle(GetCompletedWordsRequest request, CancellationToken cancellationToken)
    {
        var userId = _authService.GetCurrentUserId();
        var userTopic = await _context.UserTopics.Where(x=>
            x.UserId == userId 
            && x.TopicId == request.Id
        ).FirstOrDefaultAsync();

        if (userTopic is null)
            return FailureResponses.NotFound<List<Guid>>("User topic not found");

        return SuccessResponses.Ok(userTopic.CompleatedWordsIds);
    }
}