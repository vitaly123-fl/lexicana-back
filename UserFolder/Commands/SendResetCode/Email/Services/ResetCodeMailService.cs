using lexicana.Razor;
using lexicana.Common.Models;
using Microsoft.Extensions.Options;
using lexicana.EmailSender.Services;
using lexicana.UserFolder.Commands.SendResetCode.Email.Templates;

namespace lexicana.UserFolder.Commands.SendResetCode.Email.Services;

public class PasswordCodeModel
{
    public string Title { get; set; }
    public string UserName { get; set; }
    public string Subtitle { get; set; }
    public string Code { get; set; }
    public string LogoUrl { get; set; }
}

public record PasswordLetterModel(
    string Email, 
    string Code, 
    string UserName
);

public class ResetCodeMailService : EmailService
{
    public ResetCodeMailService(EmailSender.EmailSender emailSender, RazorRenderer razorRenderer, IOptions<AppSettingOptions> appSettings)
        : base(emailSender, razorRenderer, appSettings) { }

    public async Task SendResetCodeAsync(PasswordLetterModel model)
    {
        var emailModel = new PasswordCodeModel
        {
            Code = model.Code,
            UserName = model.UserName,
            Subtitle = "Use the code below to enter in the app.",
            Title = "We’ve received a request to reset.",
            LogoUrl = $"{_appSettings.BaseUrl}/logo/logo-with-text.png"
        };

        await SendAsync<EmailTemplate>(
            model.Email,
            "🤓 You have new message from Lexicana!",
            emailModel
        );
    }
}
