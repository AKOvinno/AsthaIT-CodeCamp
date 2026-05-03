public class Endpoint
{
    public string Path { get; set; }
    public string Method { get; set; }
    public Func<RequestContext, string> Handler; // Handler is a function that takes a RequestContext and returns a string (the response). It's a delegate that will be assigned when creating an Endpoint instance.
    public Endpoint(string path, string method, Func<RequestContext, string> handler)
    {
        Path = path;
        Method = method;
        Handler = handler;
    }
}
