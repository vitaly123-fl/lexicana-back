using lexicana.Razor;
using lexicana.EmailSender.Services;
using lexicana.TopicFolder.WordFolder.Command.SendWordReport.Email.Templates;

namespace lexicana.TopicFolder.WordFolder.Command.SendWordReport.Email.Services;

public class WordCardModel
{
    public string Word { get; set; }
    public string Translation { get; set; }
}

public class WordReportMailService : EmailService
{
    public WordReportMailService(EmailSender.EmailSender emailSender, RazorRenderer razorRenderer) 
        : base(emailSender, razorRenderer) { }

    private const string SupportEmail = "support@lexicana.app";
    
    public async Task SendTranslateReportAsync(WordCardModel cardModel)
    {
        await SendAsync<TranslateReport>(
            to: SupportEmail,
            subject: "🕵️‍♂️ Incorrect Translation Report.",
            model: cardModel
        );
    }
}
