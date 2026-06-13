namespace Decoupler.Contracts;

/// <summary>
/// Minimal CQRS abstraction used as a base for commands and queries.
/// </summary>
/// <typeparam name="TResult">Handling result type.</typeparam>
public interface IRequest<TResult>;

/// <summary>
/// Base handler interface used by command and query handlers.
/// </summary>
/// <typeparam name="TRequest">Command or query request.</typeparam>
/// <typeparam name="TResult">Handling result type.</typeparam>
public interface IRequestHandler<in TRequest, TResult>
{
    ValueTask<TResult> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}