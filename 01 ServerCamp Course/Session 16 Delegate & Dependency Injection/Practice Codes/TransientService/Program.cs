using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

var services = new ServiceCollection();

services.AddTransient<ITransientService, TransientService>();

var provider = services.BuildServiceProvider();

// We have created scope because it's a console project and we don't have scoped services in it and we don't need it in web project
using(var scope1 = provider.CreateScope())
{
    // Previously we called provider.GetRequiredService<IScopedService>() directly, .NET treats the root provider itself as a scope, so both calls hit the same "root scope" and return the same instance.
    var instance1 = scope1.ServiceProvider.GetRequiredService<ITransientService>();
    var instance2 = scope1.ServiceProvider.GetRequiredService<ITransientService>();

    Console.WriteLine("Scoped 1: ");
    Console.WriteLine($"Instance 1: {instance1.Id}");
    Console.WriteLine($"Instance 2: {instance2.Id}");
}
using(var scope2 = provider.CreateScope())
{
    var instance1 = scope2.ServiceProvider.GetRequiredService<ITransientService>();
    var instance2 = scope2.ServiceProvider.GetRequiredService<ITransientService>();

    Console.WriteLine("Scoped 2: ");
    Console.WriteLine($"Instance 1: {instance1.Id}");
    Console.WriteLine($"Instance 2: {instance2.Id}");
}

public interface ITransientService
{
    public Guid Id { get; }
}
public class TransientService : ITransientService
{
    public Guid Id { get; } = Guid.NewGuid(); // Guid means Global Unique Identifier
}