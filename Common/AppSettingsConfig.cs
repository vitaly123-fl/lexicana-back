using lexicana.Common.Models;

public static class AppSettingsConfig
{
    public static IServiceCollection AddAppSettingsConfig(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<AppSettingOptions>(
            configuration.GetSection("App")
        );
        
        return services;
    }
}