using System.Net;
using System.Net.Sockets;
using System.Text;
namespace WebServerScratch;
class RequestContext
{
    public string Method { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
}
public class TcpServer
{
    // In a TcpServer, we must need a port to listen for incoming connections. So we will have a private field to store the port number and a constructor to initialize it.
    private readonly int _port; // when naming private variables, we can use an underscore prefix to indicate that it is a private field. This is a common convention in C#.
    public TcpServer(int port)
    {
        _port = port;
    }
    // Here, async is a keyword in C# that indicates that the method is asynchronous and can be awaited. This allows us to perform asynchronous operations without blocking the main thread. In this case, we will use async to listen for incoming connections without blocking the server. Instead of returning void, we will return a Task, which represents an asynchronous operation that can be awaited. This allows us to use the await keyword when calling the StartAsync method, which will allow the server to run asynchronously and handle multiple connections concurrently.
    public async Task StartAsync() // When we will run the TcpServer, we just need to write await tcpServer.StartAsync(); in the Main method. So we will have a StartAsync method that will start the server and listen for incoming connections.
    {
        var listener = new TcpListener(IPAddress.Loopback, _port); // Here, Tcplistener is a class provided by the .NET framework that allows us to listen for incoming TCP connections on a specified port. We create an instance of TcpListener and pass the port number to its constructor. Here Operating System will automatically assign the IP address of the local machine to the listener, so it will listen on all available network interfaces.
        // Here, IPAddress.Loopback is a constant provided by the .NET framework that represents the wildcard IP address, which means that the listener will listen on all available network interfaces.
        listener.Start(); // We call the Start method on the listener to start listening for incoming connections. This will bind the listener to the specified port and allow it to accept incoming connection requests.
        Console.WriteLine($"Server started on port {_port}. Waiting for connections..."); // We can also print a message to the console to indicate that the server has started and is waiting for connections.
        while (true) // We will use an infinite loop to continuously listen for incoming connections. This allows the server to keep running and handle multiple connections concurrently.
        {
            var client = await listener.AcceptTcpClientAsync(); // Inside the loop, we call the AcceptTcpClientAsync method on the listener to asynchronously wait for an incoming connection. This method will return a TcpClient object representing the connected client once a connection is established. By using await, we can allow the server to continue processing other tasks while waiting for a connection, rather than blocking the main thread.
            Console.WriteLine("Client connected!"); // We can also print a message to the console to indicate that a client has connected.
            // Here, we can also add code to handle the connected client, such as reading data from the client or sending responses back to the client. However, for simplicity, we will just print a message when a client connects and continue listening for more connections.
            await HandleClient(client); // We can call the HandleClient method to handle the connected client. This method will take care of reading data from the client and sending responses back to the client. By using await, we can allow the server to continue listening for more connections while handling the current client asynchronously. This allows the server to handle multiple clients concurrently without blocking the main thread.
        }
    }
    private async Task HandleClient(TcpClient client) // This method will handle the connected client. It takes a TcpClient object as a parameter, which represents the connected client. We can use this method to read data from the client or send responses back to the client. This method can be called from the StartAsync method when a client connects, and it can be run asynchronously to allow the server to handle multiple clients concurrently.
    {
        // We have got the connection from the client, now we need to read data stream from the client. 
        var stream = client.GetStream(); // We can get the network stream from the TcpClient object using the GetStream method. This stream allows us to read data sent by the client and write data back to the client.
        var buffer = new byte[1024]; // We can create a buffer to store the data read from the client. In this example, we are using a buffer of size 1024 bytes, but you can adjust the size based on your needs.
        var byteCount = await stream.ReadAsync(buffer); // We can use the ReadAsync method on the stream to asynchronously read data from the client. This method will read data into the buffer and return the number of bytes read. By using await, we can allow the server to continue processing other tasks while waiting for data from the client, rather than blocking the main thread.
        var requestText = Encoding.UTF8.GetString(buffer, 0, byteCount); // We can convert the byte array in the buffer to a string using the Encoding.UTF8.GetString method. This will give us the text sent by the client. We specify the buffer, the starting index (0), and the number of bytes read (byteCount) to convert only the relevant portion of the buffer to a string.

        var lines = requestText.Split("\r\n"); // We can split the request text into lines using the Split method. This will allow us to parse the HTTP request and extract the method, path, and other information from the request. Here, \r\n is used as the delimiter to split the request into lines, as HTTP requests typically use this sequence to separate lines in the request header.

        var requestLine = lines[0].Split(" "); // We can further split the request line into its components using the Split method again. This will give us an array of strings, where the first element is the HTTP method, the second element is the path, and the third element is the HTTP version.
        // Output will be like this: ["GET", "/index.html", "HTTP/1.1"]

        var context = new RequestContext
        {
            Method = requestLine[0].ToString(),
            Path = requestLine[1].ToString()
        };
        var responseText = $"You have requested path: {context.Path} with method: {context.Method}";
        var responseBytes = Encoding.UTF8.GetBytes($"HTTP/1.1 200 OK\r\nContent-Length: {responseText.Length}\r\n\r\n{responseText}"); // We can convert the response text to a byte array using the Encoding.UTF8.GetBytes method. This will allow us to send the response back to the client over the network stream.
        await stream.WriteAsync(responseBytes); // We can use the WriteAsync method on the stream

        Console.WriteLine(responseText); // We can also print the response text to the console for debugging purposes.
        Console.WriteLine($"Received from client: {requestText}");
        client.Close(); // After handling the client, we can close the connection to free up resources. This will allow the server to continue listening for new connections while the current client is being handled.
    }
}