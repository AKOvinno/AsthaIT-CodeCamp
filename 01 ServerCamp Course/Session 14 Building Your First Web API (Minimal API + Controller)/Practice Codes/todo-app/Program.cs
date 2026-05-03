// This code follows Minimal API pattern in ASP.NET Core, which allows you to create APIs with less boilerplate code compared to traditional MVC controllers. The code defines a simple in-memory list of todos and provides endpoints to retrieve and add todos. It also integrates OpenAPI for API documentation.
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

var todos = new List<string>
{
    "1. First todo"
};

app.MapGet("/todos", (HttpContext context) => 
{
    return Results.Ok(todos);
})
.WithName("GetAllTodos"); // This is used for OpenAPI documentation to identify the endpoint with a unique name.


app.MapPost("/todo", (string title, string description) => 
{
    if (string.IsNullOrWhiteSpace(title))
    {
        return Results.BadRequest("Todo title can't be empty.");
    }
    todos.Add(title);
    return Results.Created($"/todos/{todos.Count - 1}", title);
})
.WithName("AddTodo");


app.MapPost("/todo-object", (Todo todo) =>
{
    if (string.IsNullOrWhiteSpace(todo.Title))
    {
        return Results.BadRequest("Todo title can't be empty.");
    }
    todos.Add(todo.Title);
    return Results.Created($"/todos/{todos.Count - 1}", todo.Title);
})
.WithName("AddTodoObject");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.Run();

class Todo
{
    public string Title { get; set; } = string.Empty;
}