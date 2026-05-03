using System.Reflection;

public class ServiceCollection
{
    private readonly List<Type> _controllerTypes = [];
    public void AddControllers()
    {
        var controllers = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsClass && t.Name.EndsWith("Controller"));

        _controllerTypes.AddRange(controllers);
    }
    public List<Type> GetControllerTypes() => _controllerTypes;
    // In future if we need to filter controllers based on some criteria, we can add methods here to do that. We can't filter controllers in the private field because it's kind of global and we want to keep it as a simple list of all controllers. The filtering logic can be added in the methods that retrieve the controllers, so we can have different methods for different types of controllers if needed.
}
