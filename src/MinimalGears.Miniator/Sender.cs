using System.Collections.Concurrent;
using MinimalGears.Miniator.Contracts;

namespace MinimalGears.Miniator;

public class Sender : ISender
{
    private static readonly ConcurrentDictionary<Type, IRequestHandler> _handlers = [];

    public async Task<TResponse> Send<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResponse>
    {
        var handler = _handlers[request.GetType()];
        if (handler == null) {
            throw new InvalidOperationException($"No handler registered for {request.GetType()}");
        }

        return await ((IRequestHandler<TRequest, TResponse>)handler).Handle(request, cancellationToken);
    }

    public async Task Send(IRequest request, CancellationToken cancellationToken = default)
    {
        var handler = _handlers[request.GetType()];
        if (handler == null) {
            throw new InvalidOperationException($"No handler registered for {request.GetType()}");
        }

        await ((IRequestHandler<IRequest>)handler).Handle(request, cancellationToken);
    }

    public static void RegisterHandler<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> handler)
        where TRequest : IRequest<TResponse>
    {
        _handlers.GetOrAdd(typeof(TRequest), a => handler);
    }
}
