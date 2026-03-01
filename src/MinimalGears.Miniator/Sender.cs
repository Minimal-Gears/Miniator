using System.Collections.Concurrent;
using MinimalGears.Miniator.Contracts;

namespace MinimalGears.Miniator;

public class Sender : ISender
{
    private static readonly ConcurrentDictionary<Type, Type> _handlers = [];

    public async Task<TResponse> Send<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResponse>
    {
        var handlerType = _handlers[request.GetType()];
        if (handlerType == null) {
            throw new InvalidOperationException($"No handler registered for {request.GetType()}");
        }

        var handler = Activator.CreateInstance(handlerType);
        return await ((IRequestHandler<TRequest, TResponse>)handler).Handle(request, cancellationToken);
    }

    public async Task Send(IRequest request, CancellationToken cancellationToken = default)
    {
        var handlerType = _handlers[request.GetType()];
        if (handlerType == null) {
            throw new InvalidOperationException($"No handler registered for {request.GetType()}");
        }

        var handler = Activator.CreateInstance(handlerType);
        await ((IRequestHandler<IRequest>)handler).Handle(request, cancellationToken);
    }

    public static void RegisterHandler<THandler, TRequest, TResponse>()
        where THandler : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        _handlers.GetOrAdd(typeof(TRequest), a => typeof(THandler));
    }
    
    public static void RegisterHandler(Type handlerType,Type requestType)
    {
        _handlers.GetOrAdd(handlerType, a => requestType);
    }
}
