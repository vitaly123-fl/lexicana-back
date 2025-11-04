using MediatR;
using lexicana.Database;
using lexicana.Endpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lexicana.Authorization.Services;

namespace lexicana.UserFolder.Commands.VerifyResetCode;

public record VerifyUserResetCodeRequest([FromBody] VerifyUserResetCodeBody Body) : IHttpRequest<string>;

public record VerifyUserResetCodeBody(string Email, string Code);

public class Handler: IRequestHandler<VerifyUserResetCodeRequest, Response<string>>
{
    private readonly JWtService _jWtService;
    private readonly ApplicationDbContext _context;
        
    public Handler(ApplicationDbContext context, JWtService jWtService)
    {
        _context = context;
        _jWtService = jWtService;
    }
    
    public async Task<Response<string>> Handle(VerifyUserResetCodeRequest request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x=>x.Email == request.Body.Email);

        if (user is null)
            return FailureResponses.NotFound<string>("User not found");

        if (user.ResetCode != request.Body.Code)
            return FailureResponses.NotFound<string>("The code does not match. Please try again.");

        user.ResetCode = null;
        await _context.SaveChangesAsync();

        var token = _jWtService.GenerateToken(user.Id, user.FirebaseId);
        
        return SuccessResponses.Ok(token);
    }
}