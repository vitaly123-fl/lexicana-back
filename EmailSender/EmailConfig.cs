using lexicana.Razor;
using lexicana.EmailSender.Models;
using lexicana.LessonFolder.TopicFolder.WordFolder.Command.SendWordReport.Email.Services;
using lexicana.UserFolder.Commands.SendResetCode.Email.Services;

namespace lexicana.EmailSender;

public static class EmailConfig
{
    public static IServiceCollection AddEmailConfig(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddScoped<EmailSender>();
        services.AddScoped<RazorRenderer>();
        services.AddScoped<ResetCodeMailService>();
        services.AddScoped<WordReportMailService>();
        services.Configure<EmailSenderSettings>(
            configuration.GetSection("EmailSenderSettings")
        );
        
        return services;
    }
}