using Decoupler.Application.Notes.Commands;
using Decoupler.Contracts;
using Decoupler.WebApi.Common.Endpoints;

namespace Decoupler.WebApi.Notes;

internal sealed class CreateNoteEndpoint(IDecoupler decoupler) : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/create-note", async () =>
            {
                var result = await decoupler.Send<CreateNoteCommand, Guid>(
                    new CreateNoteCommand());

                return Results.Ok(result);
            })
            .WithName("CreateNote")
            .WithTags("Notes");
    }
}