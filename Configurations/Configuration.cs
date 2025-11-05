using System.Reflection;
using lexicana.Authorization;
using lexicana.Common;
using lexicana.Database;
using lexicana.EmailSender;
using lexicana.Endpoints;

namespace lexicana.Configurations;

public static class Configuration
{
    public static IServiceCollection AddConfiguration(this IServiceCollection services, WebApplicationBuilder builder)
    {   
        var configuration = builder.Configuration;
        
        services.AddDataBaseConfig(configuration);
        services.AddCorsConfig();
        services.AddHttpContextAccessor();
        services.AddSwaggerConfig();
        services.AddEndpoints(Assembly.GetExecutingAssembly());
        services.AddAuthConfig(configuration);
        services.AddEmailConfig(configuration);
        services.AddFirebaseConfig(configuration);
        services.AddAppSettingsConfig(configuration);
        services.AddMediatR(x=> x.RegisterServicesFromAssemblyContaining<Program>());
        return services;
    }   
}       