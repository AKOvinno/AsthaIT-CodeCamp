using System.Net;
using System.Net.Sockets;
using System.Text;
public class RequestContext
{
    public string Method { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
}
public class TcpServer
{
    private readonly int _port; 
    private readonly Router _router;
    public TcpServer(int port, Router router)
    {
        _port = port;
        _router = router;
    }
    public async Task StartAsync()
    {
        var listener = new TcpListener(IPAddress.Loopback, _port);
        listener.Start();
        Console.WriteLine($"Server started on port {_port}. Waiting for connections...");
        while (true)
        {
            var client = await listener.AcceptTcpClientAsync();
            Console.WriteLine("Client connected!");
            await HandleClient(client);
        }
    }
    private async Task HandleClient(TcpClient client)
    {
        var stream = client.GetStream();
        var buffer = new byte[1024];
        var byteCount = await stream.ReadAsync(buffer);
        var requestText = Encoding.UTF8.GetString(buffer, 0, byteCount);

        var lines = requestText.Split("\r\n");

        var requestLine = lines[0].Split(" "); 

        var context = new RequestContext
        {
            Method = requestLine[0].ToString(),
            Path = requestLine[1].ToString()
        };

        // From constructor we have the instance of Router class and we are calling the Resolve method of the Router class to get the response text based on the request context. The Resolve method will check if there is a matching endpoint for the requested path and method, and if found, it will invoke the corresponding handler to generate the response text. If no matching endpoint is found, it will return a "404 Not Found" response.
        var responseText = _router.Resolve(context);

        // var responseText = $"You have requested path: {context.Path} with method: {context.Method}";

        var responseBytes = Encoding.UTF8.GetBytes(
            $"HTTP/1.1 200 OK\r\nContent-Length: {responseText.Length}\r\n\r\n{responseText}"
        );

        await stream.WriteAsync(responseBytes);

        Console.WriteLine(responseText);
        Console.WriteLine($"Received from client: {requestText}");
        client.Close();
    }
}