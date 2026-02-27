using Microsoft.Extensions.DependencyInjection;

namespace MinimalGears.Miniator.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMiniator(this IServiceCollection services)
    {
        return services;
    } 
}
