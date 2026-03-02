using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MinimalGears.Miniator.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMiniator(this IServiceCollection services, Assembly assembly)
    {
        services.AddScoped<ISender, Sender>();

        RegisterServicesFromAssembly(assembly, services);
        return services;
    }

    public static void RegisterServicesFromAssembly(Assembly assembly, IServiceCollection services)
    {
        var handlers = assembly.GetTypes().Where(t => t.IsClass && t.IsAssignableTo(typeof(IRequestHandler)));
        foreach (var handler in handlers) {
            var request = handler.GetInterfaces().First().GetGenericArguments().First();
            // var response = request.GetGenericArguments().First();
            Sender.RegisterHandler(request, handler);
            services.AddScoped(handler);
        }
    }
}
