using lexicana.Razor;
using lexicana.EmailSender.Services;
using lexicana.TopicFolder.WordFolder.Command.SendWordReport.Email.Models;
using lexicana.TopicFolder.WordFolder.Command.SendWordReport.Email.Templates;

namespace lexicana.TopicFolder.WordFolder.Command.SendWordReport.Email.Services;

public class WordReportMailService : EmailService
{
    public WordReportMailService(EmailSender.EmailSender emailSender, RazorRenderer razorRenderer) 
        : base(emailSender, razorRenderer) { }

    private const string SupportEmail = "support@lexicana.app";
    
    public async Task SendTranslateReportAsync(WordCardModel cardModel)
    {
        await SendTemplateAsync<TranslateReport>(
            to: SupportEmail,
            subject: "🕵️‍♂️ Incorrect Translation Report.",
            model: cardModel
        );
    }
}
