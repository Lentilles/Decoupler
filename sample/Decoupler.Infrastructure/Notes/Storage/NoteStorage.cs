using Decoupler.Application.Notes.Interfaces;

namespace Decoupler.Infrastructure.Notes.Storage;

public class NoteStorage : INoteStorage
{
    public ICollection<Guid> Notes { get; } = new List<Guid>();
}