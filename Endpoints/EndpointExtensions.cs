using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace lexicana.Endpoints;

public static class EndpointExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        ServiceDescriptor[] serviceDescriptors = assembly.DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false }
                           && typeof(IEndpoint).IsAssignableFrom(type))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);
        return services;
    }
 
    public static IApplicationBuilder MapEndpoints(this WebApplication app)
    {
        IEnumerable<IEndpoint> endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();
        var group = app.MapGroup("/api");
        foreach (IEndpoint endpoint in endpoints)
        {
            endpoint.MapEndpoint(group);
        }

        return app;
    }
}