using MinimalGears.Miniator.Contracts;

namespace MinimalGears.Miniator;

public interface ISender
{
    Task<TResult> Send<TRequest, TResponse, TResult>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResult>
        where TResponse : IRequestHandler<TRequest, TResult>;

    //Task Send(IRequest request, CancellationToken cancellationToken = default);
}
