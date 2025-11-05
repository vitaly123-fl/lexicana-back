using MediatR;
using lexicana.Database;
using lexicana.Endpoints;
using lexicana.Authorization.Services;
using lexicana.TopicFolder.WordFolder.Command.SendWordReport.Email.Services;

namespace lexicana.TopicFolder.WordFolder.Command.SendWordReport;

public record SendWordReportRequest(Guid Id): IHttpRequest<EmptyValue>;

public class Handler: IRequestHandler<SendWordReportRequest, Response<EmptyValue>>
{
    private readonly AuthService _authService;
    private readonly ApplicationDbContext _context;
    private readonly WordReportMailService _wordReportMailService;
    
    public Handler(ApplicationDbContext context, WordReportMailService wordReportMailService, AuthService authService)
    {
        _context = context;
        _authService = authService;
        _wordReportMailService = wordReportMailService;
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

        await _wordReportMailService.SendTranslateReportAsync(new WordCardModel()
        {
            Word = word.Value,
            Translation = word.Translation
        });

        return SuccessResponses.Ok();
    }
}