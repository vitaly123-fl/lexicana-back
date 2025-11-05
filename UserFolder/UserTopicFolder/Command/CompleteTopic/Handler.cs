using MediatR;
using lexicana.Database;
using lexicana.Endpoints;
using Microsoft.EntityFrameworkCore;
using lexicana.Authorization.Services;
using lexicana.UserFolder.UserTopicFolder.Enums;
using lexicana.UserFolder.UserTopicFolder.Entities;

namespace lexicana.UserFolder.UserTopicFolder.Command.CompleteTopic;

public record CompleteUserTopicRequest(Guid Id) : IHttpRequest<EmptyValue>;

public class Handler: IRequestHandler<CompleteUserTopicRequest, Response<EmptyValue>>
{
    private readonly AuthService _authService;
    private readonly ApplicationDbContext _context;

    public Handler(AuthService authService, ApplicationDbContext context)
    {
        _context = context;
        _authService = authService;
    }
    
    public async Task<Response<EmptyValue>> Handle(CompleteUserTopicRequest request, CancellationToken cancellationToken)
    {
        var userId = _authService.GetCurrentUserId();
        
        var userTopic = await _context.UserTopics
            .Include(x=>x.Topic)
            .FirstOrDefaultAsync(x =>
                x.TopicId == request.Id
                && x.UserId == userId
            );

        if (userTopic is null)
            return FailureResponses.NotFound("User topic not found");
        
        userTopic.CompleatedWordsIds.Clear();

        if (userTopic.Status == UserTopicStatus.Old)
        {
            await _context.SaveChangesAsync();
            return SuccessResponses.Ok();
        }
        
        userTopic.Status = UserTopicStatus.Old;
        
        var language = userTopic.Topic.Language;

        var nextTopic = await _context.Topics
            .Where(t => t.Language == language && t.Order > userTopic.Topic.Order)
            .OrderBy(t => t.Order)
            .FirstOrDefaultAsync();

        if (nextTopic is null)
        {
            return SuccessResponses.Ok();   
        }

        await _context.UserTopics.AddAsync(new UserTopic()
        {
            TopicId = nextTopic.Id,
            UserId = userTopic.UserId,
            Status = UserTopicStatus.Current
        });
        
        await _context.SaveChangesAsync();
        return SuccessResponses.Ok();
    }
}