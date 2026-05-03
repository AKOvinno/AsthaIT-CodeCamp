// Step 1
var builder = WebApplication.CreateBuilder(args); 
// Add OpenAPI services to the container like this. This will allow you to generate OpenAPI documentation for your API endpoints.
builder.Services.AddOpenApi(); // OpenAPI inject code when we add this service, but it doesn't do anything until we map it in the middleware pipeline below. This is just the setup step to ensure that the necessary services for OpenAPI are registered in the dependency injection container.

// Step 2
var app = builder.Build();

// Step 3
// Any middleware and endpoint configuration would go here. For example, you might have app.MapGet("/hello", () => "Hello World!"); to define a simple API endpoint.
if(app.Environment.IsDevelopment()) // This condition checks if the application is running in a development environment. If it is, we want to enable the OpenAPI documentation so that developers can easily access it during development. In production, you might want to disable this for security reasons or to reduce overhead.
{
    // Normally middleware starts with app.Use... but for OpenAPI, we use app.Map... to set up the endpoint for the documentation.
    app.MapOpenApi(); // This will map the OpenAPI documentation to a specific endpoint, typically /openapi or /swagger. You can customize this path if needed.

}

// Step 3
app.Run();


//  
