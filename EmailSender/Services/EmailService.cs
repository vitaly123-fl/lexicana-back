using lexicana.Razor;
using Microsoft.AspNetCore.Components;

namespace lexicana.EmailSender.Services;

public abstract class EmailService
{
    protected readonly EmailSender _emailSender;
    protected readonly RazorRenderer _razorRenderer;

    protected EmailService(EmailSender emailSender, RazorRenderer razorRenderer)
    {
        _emailSender = emailSender;
        _razorRenderer = razorRenderer;
    }

    protected async Task SendAsync(string to, string subject, string html)
    {
        var emailMessage = new EmailMessage(to, subject, html);
        await _emailSender.SendEmailAsync(emailMessage);
    }

    protected async Task SendTemplateAsync<TTemplate>(string to, string subject, object model)
        where TTemplate : IComponent
    {
        var parameters = new Dictionary<string, object?>
        {
            { "Model", model }
        };

        var html = await _razorRenderer.RenderAsync<TTemplate>(parameters);
        await SendAsync(to, subject, html);
    }
}