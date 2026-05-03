namespace WebServerScratch
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var tcpServer = new TcpServer(5006); // We can create an instance of the TcpServer class and pass the desired port number to its constructor. In this example, we are using port 5006, but you can choose any available port number that is not already in use by another application.
            await tcpServer.StartAsync(); // We can call the StartAsync method on the TcpServer instance to start the server and listen for incoming connections. Since StartAsync is an asynchronous method, we can use the await keyword to allow the server to run asynchronously and handle multiple connections concurrently.
        }
    }
}