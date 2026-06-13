using Decoupler.Contracts;
using Decoupler.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Decoupler.UnitTest;

public class ExtensionsTests
{
    [Fact]
    public void AddDecouplerFromAssembly_Registers_Decoupler()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddDecouplerFromAssembly(typeof(TestRequestHandler).Assembly);

        ServiceDescriptor? descriptor = services
            .FirstOrDefault(x => x.ServiceType == typeof(IDecoupler));

        Assert.NotNull(descriptor);
        Assert.Equal(typeof(DecouplerBase), descriptor.ImplementationType);
        Assert.Equal(ServiceLifetime.Transient, descriptor.Lifetime);
    }

    [Fact]
    public void AddDecouplerFromAssembly_Registers_RequestHandlers()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddDecouplerFromAssembly(typeof(TestRequestHandler).Assembly);

        ServiceDescriptor? descriptor = services.FirstOrDefault(x =>
            x.ServiceType == typeof(IRequestHandler<TestRequest, TestResponse>));

        Assert.NotNull(descriptor);
        Assert.Equal(typeof(TestRequestHandler), descriptor.ImplementationType);
        Assert.Equal(ServiceLifetime.Transient, descriptor.Lifetime);
    }

    [Fact]
    public void AddDecouplerFromAssembly_DoesNotRegister_AbstractHandlers()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddDecouplerFromAssembly(typeof(AbstractHandler).Assembly);

        var registered = services.Any(x => x.ImplementationType == typeof(AbstractHandler));

        Assert.False(registered);
    }

    [Fact]
    public void AddDecouplerFromAssembly_WhenNoAssembliesProvided_RegistersOnlyDecoupler()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddDecouplerFromAssembly();

        Assert.Contains(services, x => x.ServiceType == typeof(IDecoupler));

        Assert.DoesNotContain(services,
            x => x.ServiceType.IsGenericType &&
                 x.ServiceType.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));
    }

    [Fact]
    public void AddDecouplerFromAssembly_WhenAssemblyContainsNoHandlers_DoesNotRegisterHandlers()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddDecouplerFromAssembly(typeof(ExtensionsTests).Assembly);

        var handlers = services.Where(x =>
            x.ServiceType.IsGenericType &&
            x.ServiceType.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));

        Assert.Single(handlers);
    }

    [Fact]
    public void AddDecouplerFromAssembly_DoesNotRegister_Interfaces()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddDecouplerFromAssembly(typeof(IInterfaceHandler).Assembly);

        Assert.DoesNotContain(services,
            x => x.ImplementationType == typeof(IInterfaceHandler));
    }

    [Fact]
    public void AddDecouplerFromAssembly_DoesNotRegister_NonHandlerClasses()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddDecouplerFromAssembly(typeof(PlainClass).Assembly);

        Assert.DoesNotContain(services,
            x => x.ImplementationType == typeof(PlainClass));
    }

    [Fact]
    public void AddDecouplerFromAssembly_CalledTwice_DoesNotDuplicateHandlers()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddDecouplerFromAssembly(typeof(TestRequestHandler).Assembly);
        services.AddDecouplerFromAssembly(typeof(TestRequestHandler).Assembly);

        var registrations = services.Count(x =>
            x.ServiceType == typeof(IRequestHandler<TestRequest, TestResponse>) &&
            x.ImplementationType == typeof(TestRequestHandler));

        Assert.Equal(1, registrations);
    }

    private sealed class PlainClass;

    private interface IInterfaceHandler :
        IRequestHandler<TestRequest, TestResponse>;

    private sealed record TestRequest;

    private sealed record TestResponse;

    private sealed class TestRequestHandler : IRequestHandler<TestRequest, TestResponse>
    {
        public ValueTask<TestResponse> HandleAsync(TestRequest request, CancellationToken cancellationToken = default)
        {
            return ValueTask.FromResult(new TestResponse());
        }
    }

    private abstract class AbstractHandler : IRequestHandler<TestRequest, TestResponse>
    {
        public ValueTask<TestResponse> HandleAsync(TestRequest request, CancellationToken cancellationToken = default)
        {
            return ValueTask.FromResult(new TestResponse());
        }
    }
}