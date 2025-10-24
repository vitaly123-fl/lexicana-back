using System.Net;
using System.Net.Mail;
using lexicana.EmailSender.Models;
using Microsoft.Extensions.Options;

public record EmailMessage(string To, string Subject, string Html);

namespace lexicana.EmailSender
{
    public class EmailSender
    {
        private readonly EmailSenderSettings _settings;
    
        public EmailSender(IOptions<EmailSenderSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendEmailAsync(EmailMessage emailMessageBody)
        {
            try
            {
                using var smtpClient = new SmtpClient(_settings.SmtpServer, _settings.Port)
                {
                    Credentials = new NetworkCredential(_settings.Username, _settings.Password),
                    EnableSsl = _settings.EnableSsl,
                };
            
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_settings.FromEmail, _settings.FromName),
                    Subject = emailMessageBody.Subject,
                    Body = emailMessageBody.Html,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(emailMessageBody.To);
                
                await smtpClient.SendMailAsync(mailMessage);
                
                Console.WriteLine("[*] Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Failed to send email: {ex.Message}");
            }
        }
    }
}
