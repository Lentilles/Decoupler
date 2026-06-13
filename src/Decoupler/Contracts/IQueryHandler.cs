namespace Decoupler.Contracts;


/// <summary>
/// Marker interface for <see cref="IQueryHandler{TQuery,TResult}"/>
/// </summary>
/// <typeparam name="TResult">Type that will be return after handling by <see cref="IQueryHandler{TQuery,TResult}"/></typeparam>
public interface IQuery<TResult> : IRequest<TResult>;

/// <summary>
/// Dto that will be returns from <see cref="IQuery{TResult}"/>
/// </summary>
public abstract record Dto;

/// <summary>
/// Handler for queries derived by <see cref="IQuery{TResult}"/>
/// </summary>
/// <typeparam name="TQuery">
/// Type of query that will be handled. Must inherit from <see cref="IQuery{TResult}"/>.
/// </typeparam>
/// <typeparam name="TResult">
/// Type of result returned after successful query handling. Must inherit from <see cref="Dto"/>
/// </typeparam>
public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
    where TResult : Dto;