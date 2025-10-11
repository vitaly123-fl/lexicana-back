namespace lexicana.Configurations;

public static class CorsConfig
{
    public static string CorsKey="CorsPolicy";
    public static IServiceCollection AddCorsConfig(this IServiceCollection services)
    {
        services.AddCors(x=>x.AddPolicy(CorsKey, policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        }));

        return services;
    }
}
