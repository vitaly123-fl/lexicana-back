using MediatR;
using lexicana.Database;
using lexicana.Endpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lexicana.UserFolder.Commands.SendResetCode.Email.Services;

namespace lexicana.UserFolder.Commands.SendResetCode;

public record SendResetCodeRequest([FromBody] SendResetCodeBody Body) : IHttpRequest<EmptyValue>;

public record SendResetCodeBody(string Email);

public class Handler : IRequestHandler<SendResetCodeRequest, Response<EmptyValue>>
{
    private readonly ResetCodeMailService _resetCodeMailService;
    private readonly ApplicationDbContext _context;
    
    public Handler(ResetCodeMailService resetCodeMailService, ApplicationDbContext context)
    {
        _context = context;
        _resetCodeMailService = resetCodeMailService;
    }

    public async Task<Response<EmptyValue>> Handle(SendResetCodeRequest request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x=>x.Email == request.Body.Email);
        
        if (user is null) return FailureResponses.NotFound("User not found");
        
        var random = new Random();
        var code = random.Next(1000, 9999).ToString();

        user.ResetCode = code;
        
        await _context.SaveChangesAsync();
        
        await _resetCodeMailService.SendResetCodeAsync(new PasswordLetterModel(
            Code: code,
            Email: user.Email,
            UserName:user.DisplayName
        ));
        
        return SuccessResponses.Ok();
    }
}