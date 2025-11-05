using lexicana.Razor;
using lexicana.EmailSender.Services;
using lexicana.UserFolder.Commands.SendResetCode.Email.Models;
using lexicana.UserFolder.Commands.SendResetCode.Email.Templates;

namespace lexicana.UserFolder.Commands.SendResetCode.Email.Services;

public record PasswordLetterModel(
    string Email, 
    string Code, 
    string UserName
);

public class ResetCodeMailService : EmailService
{
    public ResetCodeMailService(EmailSender.EmailSender emailSender, RazorRenderer razorRenderer)
        : base(emailSender, razorRenderer) { }

    public async Task SendResetCodeAsync(PasswordLetterModel model)
    {
        var emailModel = new PasswordCodeModel
        {
            Code = model.Code,
            UserName = model.UserName,
            Subtitle = "Use the code below to enter in the app.",
            Title = "We’ve received a request to reset."
        };

        await SendTemplateAsync<EmailTemplate>(
            model.Email,
            "🤓 You have new message from Lexicana!",
            emailModel
        );
    }
}
