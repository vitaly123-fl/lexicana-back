using MediatR;
using lexicana.Database;
using lexicana.Endpoints;
using Microsoft.AspNetCore.Mvc;
using lexicana.Authorization.Services;

namespace lexicana.UserFolder.Commands.VerifyResetCode;

public record VerifyUserResetCodeRequest([FromBody] VerifyUserResetCodeBody Body) : IHttpRequest<EmptyValue>;

public record VerifyUserResetCodeBody(string Code);

public class Handler: IRequestHandler<VerifyUserResetCodeRequest, Response<EmptyValue>>
{
    private readonly AuthService _authService;
    private readonly ApplicationDbContext _context;
        
    public Handler(ApplicationDbContext context, AuthService authService)
    {
        _context = context;
        _authService = authService;
    }
    
    public async Task<Response<EmptyValue>> Handle(VerifyUserResetCodeRequest request, CancellationToken cancellationToken)
    {
        var userId = _authService.GetCurrentUserId();
        var user = await _context.Users.FindAsync(userId);

        if (user is null)
            return FailureResponses.NotFound("User not found");

        if (user.ResetCode != request.Body.Code)
            return FailureResponses.NotFound("The code does not match. Please try again.");

        user.ResetCode = null;
        await _context.SaveChangesAsync();
        return SuccessResponses.Ok();
    }
}