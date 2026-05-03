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
                            var instance = Activator.CreateInstance(controller); // new UserController();
                            var result = method.Invoke(instance, null); 
                            return result?.ToString() ?? "";
                        });
                    }
                    else if(attr.Method == "POST")
                    {
                        _router.MapPost(attr.Path, (ctx) =>
                        {
                            var instance = Activator.CreateInstance(controller); // new UserController();
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
        var tcpServer = new TcpServer(port, _router); // Endpoints are registered in the Router class and the Router instance is passed to the TcpServer constructor, allowing the server to use the registered endpoints when handling incoming requests.
        await tcpServer.StartAsync();
    }
}
// Here, we are creating MiniWebApplicationBuilder because in our dotnet program WebApplication.CreateBuilder() returns an instance of WebApplicationBuilder, and we need to have a similar structure in our implementation. The WebApplicationFactory class is created to mimic the behavior of the WebApplication class in ASP.NET Core, which is responsible for building and running the web application. In our case, it will be a simple factory class that can create instances of the web application.
// Here, MiniWebApplicationBuilder is actual wrapper around the ServiceCollection class, which is responsible for managing the services and dependencies in our application. The Build method in MiniWebApplicationBuilder is responsible for creating an instance of MiniWebApplicationBuilder, which can be used to configure the services and dependencies before building the final web application. This way, we can have a clean separation of concerns and keep our code organized and maintainable.
public class MiniWebApplicationBuilder
{
    public ServiceCollection Services { get; } = new();
    
    // Here, without using constructor injection, we are directly creating an instance of ServiceCollection inside the Build method because the constructor is private and we can't create an instance of MiniWebApplicationBuilder from outside the class. The Build Pattern build the mini web application step by step, and we want to do some configuration before we create the instance of MiniWebApplicationBuilder, so we are creating the instance inside the Build method after all the configuration is done. This way we can ensure that the instance is created with all the necessary configurations.
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