namespace Decoupler.Contracts;

/// <summary>
/// Marker interface for <see cref="ICommandHandler{TCommand,TResult}"/>
/// </summary>
/// <typeparam name="TResult">
/// Type that will be returned after handling by
/// <see cref="ICommandHandler{TCommand,TResult}"/>
/// </typeparam>
public interface ICommand<TResult> : IRequest<TResult>;

/// <summary>
/// Handler for commands derived from <see cref="Command{TResult}"/>
/// </summary>
/// <typeparam name="TCommand">
/// Type of command that will be handled. Must inherit from
/// <see cref="Command{TResult}"/>.
/// </typeparam>
/// <typeparam name="TResult">
/// Type of result returned after successful command handling.
/// </typeparam>
public interface ICommandHandler<in TCommand, TResult> : IRequestHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>;