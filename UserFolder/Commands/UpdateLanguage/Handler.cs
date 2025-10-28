using MediatR;
using lexicana.Database;
using lexicana.Endpoints;
using lexicana.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using lexicana.Authorization.Services;

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
        //TODO винеси це в сервеіс
        var userId = _authService.GetCurrentUserId();
        
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return FailureResponses.NotFound("User not found");

        user.Language = request.Body.Language;
        
        await _context.SaveChangesAsync();
        return SuccessResponses.Ok();
    }
}