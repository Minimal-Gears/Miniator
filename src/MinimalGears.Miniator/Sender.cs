using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using MinimalGears.Miniator.Contracts;

namespace MinimalGears.Miniator;

public class Sender : ISender
{
    private static readonly ConcurrentDictionary<Type, Type> _handlers = [];
    private static readonly ConcurrentDictionary<Type, Type> _behaviors = [];
    private readonly IServiceProvider _serviceProvider;

    public Sender(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> Send<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResponse>
    {
        var handlerType = _handlers[request.GetType()];
        if (handlerType == null) {
            throw new InvalidOperationException($"No handler registered for {request.GetType()}");
        }

        var handler = (IRequestHandler<TRequest, TResponse>)_serviceProvider.GetRequiredService(handlerType);

        var behaviorType = _behaviors.GetOrAdd(request.GetType(), a => null);
        if (behaviorType != null) {
            var behavior = (IPipelineBehavior<TRequest, TResponse>)_serviceProvider.GetRequiredService(behaviorType);
            return await behavior.Handle(request, async c => await handler.Handle(request, c), cancellationToken);
        }

        return await handler.Handle(request, cancellationToken);
    }

    public async Task Send(IRequest request, CancellationToken cancellationToken = default)
    {
        var handlerType = _handlers[request.GetType()];
        if (handlerType == null) {
            throw new InvalidOperationException($"No handler registered for {request.GetType()}");
        }

        var handler = (IRequestHandler<IRequest>)_serviceProvider.GetRequiredService(handlerType);
        await handler.Handle(request, cancellationToken);
    }

    public static void RegisterHandler<THandler, TRequest, TResponse>()
        where THandler : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        _handlers.GetOrAdd(typeof(TRequest), a => typeof(THandler));
    }

    public static void RegisterHandler(Type handlerType, Type requestType)
    {
        _handlers.GetOrAdd(handlerType, a => requestType);
    }

    public static void RegisterBehavior(Type handlerType, Type requestType)
    {
        _behaviors.GetOrAdd(handlerType, a => requestType);
    }
}
