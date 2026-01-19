using MediatR;
using lexicana.Database;
using lexicana.Endpoints;
using lexicana.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lexicana.Authorization.Services;
using lexicana.UserFolder.UserLessonFolder.Enums;
using lexicana.UserFolder.UserLessonFolder.Entities;

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
            .Include(x=>x.UserLessons)
            .FirstOrDefaultAsync(x=> x.Id == userId);
        
        if (user == null) 
            return FailureResponses.NotFound("User not found");

        user.Language = request.Body.Language;
        await _context.SaveChangesAsync();
        
        var hasUserLessonForLanguage = await _context.UserLessons
            .AnyAsync(ut => ut.UserId == userId && ut.Lesson.Language == request.Body.Language);

        if (hasUserLessonForLanguage) return SuccessResponses.Ok();
        
        var firstLesson = await _context.Lessons
            .Where(t => t.Language == request.Body.Language)
            .OrderBy(t => t.Order)
            .FirstOrDefaultAsync();

        if (firstLesson is null) return SuccessResponses.Ok();
        
        var userLesson = new UserLesson
        {
            UserId = user.Id,
            LessonId = firstLesson.Id,
            Status = UserLessonStatus.Current
        };

        await _context.UserLessons.AddAsync(userLesson);
        await _context.SaveChangesAsync();
        
        return SuccessResponses.Ok();
    }
}