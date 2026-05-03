using System.Reflection;

public class ServiceCollection
{
    private readonly List<Type> _controllerTypes = [];

    private readonly List<ServiceDescriptor> _services = []; // Here, This is a placeholder for the actual service registration logic.
    public void AddControllers()
    {
        var controllers = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsClass && t.Name.EndsWith("Controller"));

        _controllerTypes.AddRange(controllers);
    }
    public void AddTransient<TService, TImplementation>()
    {
        _services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifeTime.Transient));
    }
    public void AddScoped<TService, TImplementation>()
    {
        _services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifeTime.Scoped));
    }
    public void AddSingleton<TService, TImplementation>()
    {
        _services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifeTime.Singleton));
    }
    public List<Type> GetControllerTypes() => _controllerTypes;
}
