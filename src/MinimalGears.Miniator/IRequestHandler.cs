using MinimalGears.Miniator.Contracts;

namespace MinimalGears.Miniator;

public interface IRequestHandler { }

public interface IRequestHandler<TRequest> : IRequestHandler where TRequest : IRequest
{
    Task Handle(TRequest request, CancellationToken cancellationToken);
}

public interface IRequestHandler<TRequest, TResponse> : IRequestHandler where TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}
