using MediatR;
using lexicana.Database;
using lexicana.Endpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lexicana.EmailSender.Services;

namespace lexicana.UserFolder.Commands.SendResetCode;

public record SendResetCodeRequest([FromBody] SendResetCodeBody Body) : IHttpRequest<EmptyValue>;

public record SendResetCodeBody(string Email);

public class Handler : IRequestHandler<SendResetCodeRequest, Response<EmptyValue>>
{
    private readonly UserMailService _userMailService;
    private readonly ApplicationDbContext _context;
    
    public Handler(UserMailService userMailService, ApplicationDbContext context)
    {
        _context = context;
        _userMailService = userMailService;
    }

    public async Task<Response<EmptyValue>> Handle(SendResetCodeRequest request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x=>x.Email == request.Body.Email);
        
        if (user is null) return FailureResponses.NotFound("User not found");
        
        var random = new Random();
        var code = random.Next(1000, 9999).ToString();

        user.ResetCode = code;
        
        await _context.SaveChangesAsync();
        
        await _userMailService.SendResetCodeAsync(new PasswordLetterModel(
            Code: code,
            Email: user.Email,
            UserName:user.DisplayName
        ));
        
        return SuccessResponses.Ok();
    }
}