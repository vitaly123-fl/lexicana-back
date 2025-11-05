using lexicana.Razor;
using lexicana.Razor.Models;
using lexicana.Razor.Templates;

namespace lexicana.EmailSender.Services;

public class SupportMailService : EmailService
{
    public SupportMailService(EmailSender emailSender, RazorRenderer razorRenderer) 
        : base(emailSender, razorRenderer) { }

    public async Task SendTranslateReportAsync(WordCardModel cardModel)
    {
        await SendTemplateAsync<TranslateReport>(
            to: "support@lexicana.app",
            subject: "🕵️‍♂️ Incorrect Translation Report.",
            model: cardModel
        );
    }
}
