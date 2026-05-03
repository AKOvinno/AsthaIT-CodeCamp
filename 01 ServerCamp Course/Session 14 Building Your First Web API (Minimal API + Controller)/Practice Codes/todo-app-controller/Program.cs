using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddControllers(); // This line adds the necessary services for using controllers in the application. It allows us to define API endpoints using controller classes and action methods. By adding this service, we can create controllers that handle HTTP requests and return responses in our ASP.NET Core application.
var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapControllers(); // This line maps the controller routes to the application. It tells the application to use the defined controllers and their action methods to handle incoming HTTP requests. By calling this method, we enable the routing system to recognize the routes defined in our controllers and direct requests to the appropriate action methods based on the HTTP method and route patterns. Previously we added Controllers as a service, and now we are mapping those controllers to the application so that they can handle requests.
}

app.Run();

