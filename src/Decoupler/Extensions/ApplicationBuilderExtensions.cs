using System.Reflection;
using Decoupler.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Decoupler.Extensions;

public static class ApplicationBuilderExtensions
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Registers the Decoupler and all IRequestHandler implementations found in the specified assemblies.
        /// </summary>
        /// <param name="assemblies">Assemblies to scan for request handlers.</param>
        /// <returns>The IServiceCollection so that additional services can be chained.</returns>
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