using MediatR;
using lexicana.Database;
using lexicana.Endpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lexicana.Authorization.Services;

namespace lexicana.UserFolder.UserLessonFolder.Command.AddCompleteWord;

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
        
        var userLesson = await _context.UserLessons.FirstOrDefaultAsync(x=>
            x.UserId == userId 
            && x.LessonId == request.Id
        );

        if (userLesson is null)
            return FailureResponses.NotFound("User lesson not found");

        userLesson.CompleatedWordsIds.Add(request.Body.WordId);
        await _context.SaveChangesAsync();
        return SuccessResponses.Ok();
    }
}