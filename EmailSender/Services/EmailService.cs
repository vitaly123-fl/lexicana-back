using lexicana.Razor;
using lexicana.Common.Models;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Components;

namespace lexicana.EmailSender.Services;

public abstract class EmailService
{
    private readonly EmailSender _emailSender;
    private readonly RazorRenderer _razorRenderer;
    protected readonly AppSettingOptions _appSettings;

    public EmailService(EmailSender emailSender, RazorRenderer razorRenderer, IOptions<AppSettingOptions> appSettings)
    {
        _emailSender = emailSender;
        _razorRenderer = razorRenderer;
        _appSettings = appSettings.Value;
    }

    private async Task SendAsync(string to, string subject, string html)
    {
        var emailMessage = new EmailMessage(to, subject, html);
        await _emailSender.SendEmailAsync(emailMessage);
    }

    protected async Task SendAsync<TTemplate>(string to, string subject, object model)
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