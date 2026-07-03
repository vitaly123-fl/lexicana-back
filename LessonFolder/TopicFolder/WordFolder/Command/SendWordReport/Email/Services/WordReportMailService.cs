using lexicana.Razor;
using lexicana.Common.Enums;
using lexicana.EmailSender.Services;
using lexicana.LessonFolder.TopicFolder.WordFolder.Command.SendWordReport.Email.Templates;

namespace lexicana.LessonFolder.TopicFolder.WordFolder.Command.SendWordReport.Email.Services;

public class WordCardModel
{
    public string Word { get; set; }
    public string Email { get; set; }
    public Language Language { get; set; }
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
