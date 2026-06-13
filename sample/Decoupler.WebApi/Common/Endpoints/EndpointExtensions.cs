using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Decoupler.WebApi.Common.Endpoints;

public static class EndpointExtensions
{
    public static IServiceCollection AddEndpoints(
        this IServiceCollection services, 
        params Assembly[] assemblies)
    {
        ServiceDescriptor[] serviceDescriptors = [.. assemblies
            .SelectMany(a => a.GetTypes())
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                           type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))];

        if (serviceDescriptors.Length == 0)
        {
            Console.WriteLine("No endpoints found.");
        }
        
        services.TryAddEnumerable(serviceDescriptors);

        return services;
    }

    public static IApplicationBuilder MapEndpoints(
        this WebApplication app,
        RouteGroupBuilder? routeGroupBuilder = null)
    {
        IEnumerable<IEndpoint> endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        IEndpointRouteBuilder builder = routeGroupBuilder is null ? app : routeGroupBuilder;

        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(builder);
        }

        return app;
    }
}