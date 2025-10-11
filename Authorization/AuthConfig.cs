using lexicana.Authorization.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace lexicana.Authorization;

public static class AuthConfig
{
    public static IServiceCollection AddAuthConfig(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<AuthOptions>(configuration.GetSection(AuthOptions.Jwt));
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var authOptions = configuration.GetSection(AuthOptions.Jwt).Get<AuthOptions>();

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = authOptions.Issuer,
                    ValidAudience = authOptions.Audience,
                    IssuerSigningKey = authOptions.GetSymmetricSecurityKey()
                };
            });

        services.AddAuthorization();
        services.AddSingleton<JWtService>();
        services.AddScoped<AuthService>();
        return services;
    }
}