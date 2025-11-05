using MediatR;
using lexicana.Database;
using lexicana.Endpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lexicana.Authorization.Services;

namespace lexicana.UserFolder.UserTopicFolder.Command.AddCompleteWord;

public record AddCompleteWordRequest(Guid Id, [FromBody] AddCompleteWordBody Body) : IHttpRequest<EmptyValue>;

public record AddCompleteWordBody(Guid WordId);

public class Handler: IRequestHandler<AddCompleteWordRequest, Response<EmptyValue>>
{
    private readonly AuthService _authService;
    private readonly ApplicationDbContext _context;
    
    public Handler(ApplicationDbContext context, AuthService authService)
    {
        _context = context;
        _authService = authService;
    }
    
    public async Task<Response<EmptyValue>> Handle(AddCompleteWordRequest request, CancellationToken cancellationToken)
    {
        var userId = _authService.GetCurrentUserId();
        var userTopic = await _context.UserTopics.FirstOrDefaultAsync(x=>
            x.UserId == userId 
            && x.TopicId == request.Id
        );

        if (userTopic is null)
            return FailureResponses.NotFound("User topic not found");

        userTopic.CompleatedWordsIds.Add(request.Body.WordId);
        await _context.SaveChangesAsync();
        return SuccessResponses.Ok();
    }
}