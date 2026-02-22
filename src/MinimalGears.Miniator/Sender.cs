using MinimalGears.Miniator.Contracts;

namespace MinimalGears.Miniator;

public class Sender : ISender
{
    //private readonly Dictionary<Type, IRequestHandler> _handlers;
    public Dictionary<Type, IRequestHandler> _handlers;

    public async Task<TResult> Send<TRequest, TResponse, TResult>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResult> where TResponse : IRequestHandler<TRequest, TResult>
    {
        var handler = _handlers[request.GetType()];
        if (handler == null) {
            throw new InvalidOperationException($"No handler registered for {request.GetType()}");
        }

        return await ((IRequestHandler<TRequest, TResult>)handler).Handle(request, cancellationToken);
    }

    public async Task Send(IRequest request, CancellationToken cancellationToken = default)
    {
        var handler = _handlers[request.GetType()];
        if (handler == null) {
            throw new InvalidOperationException($"No handler registered for {request.GetType()}");
        }

        await ((IRequestHandler<IRequest>)handler).Handle(request, cancellationToken);
    }
}
