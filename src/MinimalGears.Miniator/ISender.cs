using MinimalGears.Miniator.Contracts;

namespace MinimalGears.Miniator;

public interface ISender
{
    Task<TResponse> Send<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResponse>;

    //Task Send(IRequest request, CancellationToken cancellationToken = default);
}
    