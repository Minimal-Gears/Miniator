namespace MinimalGears.Miniator;

public interface IPipelineBehavior<in TRequest, TResponse>
{
    Task<TResponse> Handle(TRequest request, HandleRequestDelegate<TResponse> next, CancellationToken cancellationToken);
}

public delegate Task<TResponse> HandleRequestDelegate<TResponse>(CancellationToken cancellationToken);
