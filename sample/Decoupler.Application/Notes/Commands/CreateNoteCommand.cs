using Decoupler.Application.Notes.Interfaces;
using Decoupler.Contracts;

namespace Decoupler.Application.Notes.Commands;

public record CreateNoteCommand : ICommand<Guid>;

internal class CreateNoteCommandHandler(INoteStorage storage) : ICommandHandler<CreateNoteCommand, Guid>
{
    public ValueTask<Guid> HandleAsync(CreateNoteCommand command, CancellationToken token = default)
    {
        var guid = Guid.NewGuid();
        storage.Notes.Add(guid);
        return ValueTask.FromResult(guid);
    }
}