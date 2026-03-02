using System.Reflection;

namespace MinimalGears.Miniator.DependencyInjection;

public class MiniatorServiceConfiguration
{
    internal List<Assembly> AssembliesToRegister { get; set; } = [];

    public void RegisterServicesFromAssembly(Assembly assembly)
    {
        AssembliesToRegister.Add(assembly);
    }
}
