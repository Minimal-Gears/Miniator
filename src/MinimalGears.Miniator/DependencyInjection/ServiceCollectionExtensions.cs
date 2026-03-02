using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MinimalGears.Miniator.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMiniator(this IServiceCollection services, Action<MiniatorServiceConfiguration> configuration)
    {
        MiniatorServiceConfiguration config = new MiniatorServiceConfiguration();
        configuration.Invoke(config);
        foreach (var assembly in config.AssembliesToRegister) {
            RegisterServicesFromAssembly(assembly, services);
        }

        services.AddScoped<ISender, Sender>();
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
