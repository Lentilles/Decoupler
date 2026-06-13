namespace Decoupler.Application.Notes.Interfaces;

public interface INoteStorage
{
    public ICollection<Guid> Notes { get; }
}