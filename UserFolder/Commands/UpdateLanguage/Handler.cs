using MediatR;
using lexicana.Database;
using lexicana.Endpoints;
using lexicana.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lexicana.Authorization.Services;
using lexicana.UserFolder.UserTopicFolder.Enums;
using lexicana.UserFolder.UserTopicFolder.Entities;

namespace lexicana.UserFolder.Commands.UpdateLanguage;

public record UpdateUserLanguageRequest([FromBody] UpdateUserLanguageBody Body) : IHttpRequest<EmptyValue>;

public record UpdateUserLanguageBody(
    Language Language    
);

public class Handler: IRequestHandler<UpdateUserLanguageRequest, Response<EmptyValue>>
{
    private readonly AuthService _authService;
    private readonly ApplicationDbContext _context;

    public Handler(AuthService authService, ApplicationDbContext context)
    {
        _context = context;
        _authService = authService;
    }
    
    public async Task<Response<EmptyValue>> Handle(UpdateUserLanguageRequest request, CancellationToken cancellationToken)
    {
        var userId = _authService.GetCurrentUserId();
        
        var user = await _context.Users
            .Include(x=>x.UserTopics)
            .FirstOrDefaultAsync(x=> x.Id == userId);
        
        if (user == null) 
            return FailureResponses.NotFound("User not found");

        user.Language = request.Body.Language;
        await _context.SaveChangesAsync();
        
        var hasUserTopicForLanguage = await _context.UserTopics
            .AnyAsync(ut => ut.UserId == userId && ut.Topic.Language == request.Body.Language);

        if (hasUserTopicForLanguage) return SuccessResponses.Ok();
        
        var firstTopic = await _context.Topics
            .Where(t => t.Language == request.Body.Language)
            .OrderBy(t => t.Order)
            .FirstOrDefaultAsync();

        if (firstTopic is null) return SuccessResponses.Ok();
        
        var userTopic = new UserTopic
        {
            UserId = user.Id,
            TopicId = firstTopic.Id,
            Status = UserTopicStatus.Current
        };

        await _context.UserTopics.AddAsync(userTopic);
        await _context.SaveChangesAsync();
        return SuccessResponses.Ok();
    }
}