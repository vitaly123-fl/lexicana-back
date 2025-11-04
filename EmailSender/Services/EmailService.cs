using lexicana.Razor;
using lexicana.Razor.Models;
using lexicana.Razor.Templates;

namespace lexicana.EmailSender.Services;

public record PasswordLetterModel(
    string Email, 
    string Code, 
    string UserName
);

public class EmailService
{
    private readonly EmailSender _emailSender;
    private readonly RazorRenderer _razorRenderer;

    public EmailService(EmailSender emailSender, RazorRenderer razorRenderer)
    {
        _emailSender = emailSender;
        _razorRenderer = razorRenderer;
    }

    public async Task SendResetCodeAsync(PasswordLetterModel model)
    {
        var emailModel = new PasswordCodeModel
        {
            Code = model.Code,
            UserName = model.UserName,
            Subtitle = "Use the code below to enter in the app.",
            Title = "We’ve received a request to reset."
        };
        
        var parameters = new Dictionary<string, object?>
        {
            { "Model", emailModel }
        };

        var html = await _razorRenderer.RenderAsync<EmailTemplate>(parameters);

        var emailMessage = new EmailMessage(
            To: model.Email,
            Subject: "🤓 You have new message from Lexicana!",
            Html: html
        );

        await _emailSender.SendEmailAsync(emailMessage);
    }
    
    public async Task SendTranslateReportAsync(WordCardModel cardModel)
    {
        var parameters = new Dictionary<string, object?>
        {
            { "Model", cardModel }
        };

        var html = await _razorRenderer.RenderAsync<TranslateReport>(parameters);

        var emailMessage = new EmailMessage(
            To: "support@lexicana.app",
            Subject: "🕵️‍♂️ Incorrect Translation Report.",
            Html: html
        );

        await _emailSender.SendEmailAsync(emailMessage);
    }
}