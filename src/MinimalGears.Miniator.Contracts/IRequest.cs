namespace MinimalGears.Miniator.Contracts;

public interface IRequest { }

public interface IRequest<out TResponse> : IRequest { }
