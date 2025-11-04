using MediatR;
using lexicana.Database;
using lexicana.Endpoints;
using lexicana.Razor.Models;
using lexicana.EmailSender.Services;
using lexicana.Authorization.Services;

namespace lexicana.TopicFolder.WordFolder.Command.SendWordReport;

public record SendWordReportRequest(Guid Id): IHttpRequest<EmptyValue>;

public class Handler: IRequestHandler<SendWordReportRequest, Response<EmptyValue>>
{
    private readonly AuthService _authService;
    private readonly EmailService _emailService;
    private readonly ApplicationDbContext _context;
    
    public Handler(ApplicationDbContext context, EmailService emailService, AuthService authService)
    {
        _context = context;
        _authService = authService;
        _emailService = emailService;
    }
        
    public async Task<Response<EmptyValue>> Handle(SendWordReportRequest request, CancellationToken cancellationToken)
    {
        var userId = _authService.GetCurrentUserId();
        var user = await _context.Users.FindAsync(userId);

        if (user is null)
            return FailureResponses.NotFound("User not found");
        
        var word = await _context.Words.FindAsync(request.Id);

        if (word is null)
            return FailureResponses.NotFound("Word not found");

        await _emailService.SendTranslateReportAsync(new WordCardModel()
        {
            Word = word.Value,
            Translation = word.Translation
        });

        return SuccessResponses.Ok();
    }
}