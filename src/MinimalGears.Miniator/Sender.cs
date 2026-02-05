using MinimalGears.Miniator.Contracts;

namespace MinimalGears.Miniator;

public class Sender : ISender
{
    private readonly Dictionary<Type, IRequestHandler> _handlers;

    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var handler = _handlers[request.GetType()];
        if (handler == null) {
            throw new InvalidOperationException($"No handler registered for {request.GetType()}");
        }

        return await ((IRequestHandler<IRequest<TResponse>, TResponse>)handler).Handle(request, cancellationToken);
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
