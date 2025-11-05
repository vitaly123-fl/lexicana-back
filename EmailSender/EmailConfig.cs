using lexicana.Razor;
using lexicana.EmailSender.Models;
using lexicana.EmailSender.Services;

namespace lexicana.EmailSender;

public static class EmailConfig
{
    public static IServiceCollection AddEmailConfig(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddScoped<EmailSender>();
        services.AddScoped<RazorRenderer>();
        services.AddScoped<UserMailService>();
        services.AddScoped<SupportMailService>();
        services.Configure<EmailSenderSettings>(
            configuration.GetSection("EmailSenderSettings")
        );
        
        return services;
    }
}