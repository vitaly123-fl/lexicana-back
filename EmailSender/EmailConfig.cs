using lexicana.EmailSender.Models;
using lexicana.EmailSender.Services;
using lexicana.Razor;

namespace lexicana.EmailSender;

public static class EmailConfig
{
    public static IServiceCollection AddEmailConfig(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddScoped<EmailSender>();
        services.AddScoped<EmailService>();   
        services.AddScoped<RazorRenderer>();
        services.Configure<EmailSenderSettings>(
            configuration.GetSection("EmailSenderSettings")
        );
        
        return services;
    }
}