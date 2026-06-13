namespace Decoupler.Contracts;

public interface IRequest<TResult>;

public interface IRequestHandler<in TRequest, TResult>
{
    ValueTask<TResult> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}