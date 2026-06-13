using System.Reflection;
using Decoupler.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Decoupler.Extensions;

public static class ApplicationBuilderExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddDecouplerFromAssembly(params Assembly[] assemblies)
        {
            services.AddTransient<IDecoupler, DecouplerBase>();
            
            ServiceDescriptor[] serviceDescriptors = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(type => type is { IsAbstract: false, IsInterface: false })
                .SelectMany(type => type.GetInterfaces()
                    .Where(i => i.IsGenericType &&
                                i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
                    .Select(i => ServiceDescriptor.Transient(i, type)))
                .ToArray();
            
            if (serviceDescriptors.Length == 0)
            {
                Console.WriteLine("No handlers found.");
            }
            
            services.TryAddEnumerable(serviceDescriptors);
            
            return services;
        }
    }
}