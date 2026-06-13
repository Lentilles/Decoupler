using Microsoft.Extensions.DependencyInjection;

namespace Decoupler.Contracts;

/// <summary>
/// Provides a mechanism for dispatching requests to their respective handlers.
/// </summary>
public interface IDecoupler
{
    /// <summary>
    /// Sends a request to a handler and returns the result of its execution.
    /// </summary>
    /// <typeparam name="TRequest">Request type.</typeparam>
    /// <typeparam name="TResult">Result type returned by request processing.</typeparam>
    /// <param name="request">Request instance.</param>
    /// <param name="cancellationToken">
    /// Cancellation token that allows the operation to be cancelled.
    /// </param>
    /// <returns>
    /// Result of the request processing.
    /// </returns>
    ValueTask<TResult> Send<TRequest, TResult>(
        TRequest request,
        CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResult>;
}

/// <summary>
/// Base implementation of <see cref="IDecoupler"/>
/// that resolves handlers via the dependency injection container.
/// </summary>
/// <param name="scopeFactory">
/// Scope factory used to create a separate scope
/// for each request execution.
/// </param>
public class DecouplerBase(IServiceScopeFactory scopeFactory) : IDecoupler
{
    /// <summary>
    /// Creates a new scope, resolves the request handler,
    /// and delegates execution to it.
    /// </summary>
    /// <typeparam name="TRequest">Request type.</typeparam>
    /// <typeparam name="TResult">Result type returned by request processing.</typeparam>
    /// <param name="request">Request instance.</param>
    /// <param name="cancellationToken">
    /// Cancellation token that allows the operation to be cancelled.</param>
    /// <returns>
    /// Result returned by the request handler.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if no handler for the specified request type is registered in the dependency injection container.
    /// </exception>
    public virtual async ValueTask<TResult> Send<TRequest, TResult>(
        TRequest request,
        CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResult>
    {
        using var scope = scopeFactory.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<IRequestHandler<TRequest, TResult>>();
        var result = await handler.HandleAsync(request, cancellationToken);
        return result;
    }
}