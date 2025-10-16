namespace lexicana.EmailSender.Models;

public class EmailSenderSettings
{
    public string SmtpServer { get; set; } = string.Empty;
    public int Port { get; set; } = 587; // 587 для TLS, 465 для SSL
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FromEmail { get; set; } = string.Empty;
    public string FromName { get; set; } = "Lexicana";
    public bool EnableSsl { get; set; } = true;
}