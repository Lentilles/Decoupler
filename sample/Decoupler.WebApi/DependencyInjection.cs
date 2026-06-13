using Decoupler.WebApi.Common.Endpoints;

namespace Decoupler.WebApi;

public static class DependencyInjection
{
    extension(IServiceCollection collection)
    {
        public IServiceCollection AddPresentationLayer()
        {
            collection.AddEndpoints(AssemblyReference.GetAssembly());
            return collection;
        }
    }
}