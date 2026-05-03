using System.Reflection;

public class MiniWebApplication 
{
    private readonly ServiceCollection _services;
    private readonly Router _router = new();

    public MiniWebApplication(ServiceCollection services)
    {
        _services = services;
    }

    public MiniWebApplication MapGet(string path, Func<RequestContext, string> handler)
    {
        _router.MapGet(path, handler);
        return this;
    }
    public MiniWebApplication MapControllers()
    {
        var controllerTypes = _services.GetControllerTypes();
        foreach(var controller in controllerTypes)
        {
            var methods = controller.GetMethods(BindingFlags.Instance | BindingFlags.Public);

            foreach (var method in methods)
            {
                var attr = method.GetCustomAttributes<HttpMethodAttribute>().FirstOrDefault();
                if(attr != null)
                {
                    if(attr.Method == "GET")
                    {
                        _router.MapGet(attr.Path, (ctx) =>
                        {
                            var instance = Activator.CreateInstance(controller);
                            var result = method.Invoke(instance, null); 
                            return result?.ToString() ?? "";
                        });
                    }
                    else if(attr.Method == "POST")
                    {
                        _router.MapPost(attr.Path, (ctx) =>
                        {
                            var instance = Activator.CreateInstance(controller);
                            var result = method.Invoke(instance, null); 
                            return result?.ToString() ?? "";
                        });
                    }
                }
            }
        }
        return this;
    }

    public async Task RunAsync(int port)
    {
        var tcpServer = new TcpServer(port, _router);
        await tcpServer.StartAsync();
    }
}
public class MiniWebApplicationBuilder
{
    public ServiceCollection Services { get; } = new();
    public MiniWebApplication Build()
    {
        return new MiniWebApplication(Services);
    }
}
public class WebApplicationFactory
{
    public static MiniWebApplicationBuilder CreateBuilder()
    {
        return new MiniWebApplicationBuilder();
    }
}