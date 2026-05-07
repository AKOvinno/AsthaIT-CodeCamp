using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=codecampdb";
// var npgsqlConnection = new NpgsqlConnection(connectionString); 

var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=root;Database=codecampdb";

// The 'using' statement ensures the connection is closed automatically
using (var npgsqlConnection = new NpgsqlConnection(connectionString))
{
    try 
    {
        await npgsqlConnection.OpenAsync();
        Console.WriteLine("Connection Opened Successfully!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
} // <--- npgsqlConnection.Dispose() is called here automatically

var app = builder.Build();

app.MapPost("/todos", async (string title) =>
{
    var todoService = new TodoService();
    // 2. Create the connection INSIDE the request handler
    // This ensures every request gets its own fresh connection
    using (var npgsqlConnection = new NpgsqlConnection(connectionString))
    {
        await npgsqlConnection.OpenAsync();
        await todoService.AddTodoAsync(title, npgsqlConnection);
        await npgsqlConnection.DisposeAsync();
    }
    return Results.Ok( new { message = "Todo added successfully!" });
});
app.MapGet("/todos/{id}", async (int id) =>
{
    using (var npgsqlConnection = new NpgsqlConnection(connectionString))
    {
        await npgsqlConnection.OpenAsync();
        var todoService = new TodoService();
        
        var title = await todoService.GetTodoTitleAsync(id, npgsqlConnection);

        if (title == null)
        {
            return Results.NotFound(new { message = $"Todo with ID {id} not found." });
        }
        return Results.Ok(new { 
            message = "Retrieved successfully", 
            title = title 
        });
    }
});
app.MapDelete("/todos/{id}", async (int id) =>
{
    using (var npgsqlConnection = new NpgsqlConnection(connectionString))
    {
        await npgsqlConnection.OpenAsync();
        var todoService = new TodoService();
        bool isDeleted = await todoService.DeleteTodoAsync(id, npgsqlConnection);
        if(!isDeleted) return Results.NotFound(new { message = $"Todo with ID {id} not found." });
        return Results.Ok(new { message = "Deleted successfully" });
    }
});
app.MapPut("/todos/{id}", async (int id, string title) =>
{
    using (var npgsqlConnection = new NpgsqlConnection(connectionString))
    {
        await npgsqlConnection.OpenAsync();
        var todoService = new TodoService();
        bool isUpdated = await todoService.UpdateTodoAsync(id, title, npgsqlConnection);
        if(!isUpdated) return Results.NotFound(new { message = $"Todo with ID {id} not found." });
        return Results.Ok(new { message = "Updated successfully" });
    }
});

app.Run();

