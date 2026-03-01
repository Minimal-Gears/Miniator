using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MinimalGears.Miniator.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMiniator(this IServiceCollection services, Assembly assembly)
    {
        RegisterServicesFromAssembly(assembly);
        return services;
    }

    public static void RegisterServicesFromAssembly(Assembly assembly)
    {
        var handlers = assembly.GetTypes().Where(t => t.IsClass && t.IsAssignableTo(typeof(IRequestHandler)));
        foreach (var handler in handlers) {
            var request = handler.GetInterfaces().First().GetGenericArguments().First();
            // var response = request.GetGenericArguments().First();
            Sender.RegisterHandler(request, handler);
        }
    }
}
