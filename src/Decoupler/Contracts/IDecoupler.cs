using Microsoft.Extensions.DependencyInjection;

namespace Decoupler.Contracts;

public interface IDecoupler
{
    ValueTask<TResult> Send<TRequest, TResult>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResult>;
}

public class DecouplerBase(IServiceScopeFactory scopeFactory) : IDecoupler
{
    public virtual async ValueTask<TResult> Send<TRequest, TResult>(TRequest request,
        CancellationToken cancellationToken = default) where TRequest : IRequest<TResult>
    {
        using var scope = scopeFactory.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<IRequestHandler<TRequest, TResult>>();
        var result = await handler.HandleAsync(request, cancellationToken);
        return result;
    }
}