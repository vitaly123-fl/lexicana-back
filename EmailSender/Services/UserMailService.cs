using lexicana.Razor;
using lexicana.Razor.Models;
using lexicana.Razor.Templates;

namespace lexicana.EmailSender.Services;

public record PasswordLetterModel(
    string Email, 
    string Code, 
    string UserName
);

public class UserMailService : EmailService
{
    public UserMailService(EmailSender emailSender, RazorRenderer razorRenderer)
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
