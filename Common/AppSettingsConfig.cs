using lexicana.Common.Models;

namespace lexicana.Common;

public static class AppSettingsConfig
{
    public static IServiceCollection AddAppSettingsConfig(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<AppSettingOptions>(
            configuration.GetSection(AppSettingOptions.Key)
        );
        
        return services;
    }
}