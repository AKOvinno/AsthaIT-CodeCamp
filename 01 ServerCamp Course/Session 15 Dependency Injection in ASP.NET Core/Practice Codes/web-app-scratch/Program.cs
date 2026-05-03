var builder = WebApplicationFactory.CreateBuilder();
builder.Services.AddControllers();

var app = builder.Build();

//var router = new Router();

// router.MapGet("/codecamp", (ctx) =>
// {
//     Console.WriteLine("Received request for /codecamp");
//     return "Hello CodeCamp!";
// });

app.MapGet("/codecamp", (ctx) =>
{
    Console.WriteLine("Received request for /codecamp");
    return "Hello CodeCamp!";
});

app.MapControllers();

// var tcpServer = new TcpServer(5006, ); // Endpoints are registered in the Router class and the Router instance is passed to the TcpServer constructor, allowing the server to use the registered endpoints when handling incoming requests.
// await tcpServer.StartAsync();

await app.RunAsync(5006);
