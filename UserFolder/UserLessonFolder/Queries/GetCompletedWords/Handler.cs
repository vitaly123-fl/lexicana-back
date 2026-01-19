using MediatR;
using lexicana.Database;
using lexicana.Endpoints;
using Microsoft.EntityFrameworkCore;
using lexicana.Authorization.Services;

namespace lexicana.UserFolder.UserLessonFolder.Queries.GetCompletedWords;

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
        
        var userLesson = await _context.UserLessons.Where(x=>
            x.UserId == userId 
            && x.LessonId == request.Id
        ).FirstOrDefaultAsync();

        if (userLesson is null)
            return FailureResponses.NotFound<List<Guid>>("User lesson not found");

        return SuccessResponses.Ok(userLesson.CompleatedWordsIds);
    }
}