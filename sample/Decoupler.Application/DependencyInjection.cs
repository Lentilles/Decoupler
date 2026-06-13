using Decoupler.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Decoupler.Application;

public static class DependencyInjection
{
    extension(IServiceCollection collection)
    {
        public IServiceCollection AddApplicationLayer()
        {
            collection.AddDecouplerFromAssembly(AssemblyReference.GetAssembly());
            return collection;
        }
    }
}