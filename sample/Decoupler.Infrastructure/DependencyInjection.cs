using Decoupler.Application.Notes.Interfaces;
using Decoupler.Infrastructure.Notes.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Decoupler.Infrastructure;

public static class DependencyInjection
{
    extension(IServiceCollection collection)
    {
        public IServiceCollection AddInfrastructureLayer()
        {
            collection.AddSingleton<INoteStorage, NoteStorage>();
            return collection;
        }
    }
}