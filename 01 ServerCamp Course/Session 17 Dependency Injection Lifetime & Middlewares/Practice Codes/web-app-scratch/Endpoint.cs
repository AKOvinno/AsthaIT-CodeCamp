public class Endpoint
{
    public string Path { get; set; }
    public string Method { get; set; }
    public Func<RequestContext, string> Handler; 
    public Endpoint(string path, string method, Func<RequestContext, string> handler)
    {
        Path = path;
        Method = method;
        Handler = handler;
    }
}
