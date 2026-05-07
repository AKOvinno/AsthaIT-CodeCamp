using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Application and database connection for two lines
var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=root;Database=codecampdb";
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

var app = builder.Build();
app.MapPost("/user", async(AppDbContext context, UserRequest request) =>
{
    var user = new User
    {
        Name = request.Name,
        Email = request.Email
    };
    var entriesBeforeAdd = context.ChangeTracker.Entries()
        .Select(e => new
        {
            EntityName = e.Entity.GetType().Name,
            State = e.State.ToString()
        });
    Console.WriteLine("\n------Entries Before Add ------");
    foreach(var entity in entriesBeforeAdd)
    {
        Console.WriteLine($"Entity: {entity.EntityName}");
        Console.WriteLine($"State: {entity.State}");
        Console.WriteLine("--------------------------------------");
    }

    await context.Users.AddAsync(user);

    var entriesAfterAdd = context.ChangeTracker.Entries()
        .Select(e => new
        {
            EntityName = e.Entity.GetType().Name,
            State = e.State.ToString()
        });
    Console.WriteLine("\n------Entries After Add ------");
    foreach(var entity in entriesAfterAdd)
    {
        Console.WriteLine($"Entity: {entity.EntityName}");
        Console.WriteLine($"State: {entity.State}");
        Console.WriteLine("--------------------------------------");
    }

    await context.SaveChangesAsync();

    var entriesAfterSaveChange = context.ChangeTracker.Entries()
        .Select(e => new
        {
            EntityName = e.Entity.GetType().Name,
            State = e.State.ToString()
        });

    Console.WriteLine("\n------Entries After Save Change ------");
    foreach(var entry in entriesAfterSaveChange)
    {
        Console.WriteLine($"Entity: {entry.EntityName}");
        Console.WriteLine($"State: {entry.State}");
        Console.WriteLine("--------------------------------------");
    }
});
app.Run();


