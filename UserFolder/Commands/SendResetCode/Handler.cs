using MediatR;
using lexicana.Database;
using lexicana.Endpoints;
using lexicana.Razor.Models;
using lexicana.EmailSender.Services;
using lexicana.Authorization.Services;

namespace lexicana.UserFolder.Commands.SendResetCode;

public record SendResetCodeRequest() : IHttpRequest<EmptyValue>;

public class Handler : IRequestHandler<SendResetCodeRequest, Response<EmptyValue>>
{
    private readonly AuthService _authService;
    private readonly EmailService _emailService;
    private readonly ApplicationDbContext _context;
    
    public Handler(EmailService emailService, ApplicationDbContext context, AuthService authService)
    {
        _context = context;
        _authService = authService;
        _emailService = emailService;
    }

    public async Task<Response<EmptyValue>> Handle(SendResetCodeRequest request, CancellationToken cancellationToken)
    {
        var userId = _authService.GetCurrentUserId();
        var user = await _context.Users.FindAsync(userId);
        
        if (user is null) return FailureResponses.NotFound("User not found");
        
        var random = new Random();
        var code = random.Next(1000, 9999).ToString();

        user.ResetCode = code;
        
        await _context.SaveChangesAsync();
        
        await _emailService.SendResetCodeAsync(new PasswordLetterModel(
            Code: code,
            Email: user.Email,
            UserName:user.DisplayName
        ));
        
        return SuccessResponses.Ok();
    }
}