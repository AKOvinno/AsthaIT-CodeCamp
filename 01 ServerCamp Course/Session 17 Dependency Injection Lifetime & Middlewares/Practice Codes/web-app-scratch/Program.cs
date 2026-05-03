var builder = WebApplicationFactory.CreateBuilder();
builder.Services.AddControllers();

builder.Services.AddTransient<IMyTransientService, MyTransientService>();
builder.Services.AddScoped<IMyScopedService, MyScopedService>();
builder.Services.AddSingleton<IMySingletonService, MySingletonService>();

var app = builder.Build();


app.MapGet("/codecamp", (ctx) =>
{
    Console.WriteLine("Received request for /codecamp");
    return "Hello CodeCamp!";
});

app.MapControllers();

// app.Services.GetRequireService<IMyTransientService>();
// app.Services.GetRequireService<IMyScopedService>();
// app.Services.GetRequireService<IMySingletonService>();

await app.RunAsync(5006);

public interface IMyTransientService { }
public interface IMyScopedService { }
public interface IMySingletonService { }
public class MyTransientService : IMyTransientService { }
public class MyScopedService : IMyScopedService { }
public class MySingletonService : IMySingletonService { }