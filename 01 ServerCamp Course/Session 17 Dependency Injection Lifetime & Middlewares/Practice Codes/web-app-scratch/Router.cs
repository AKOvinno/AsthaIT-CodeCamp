public class Router
{
    private readonly List<Endpoint> _endpoints = []; 
    public void MapGet(string path, Func<RequestContext, string> handler)
    {
        _endpoints.Add(new Endpoint(path, "GET", handler));
    }
    public void MapGet(string path, Action<RequestContext> handler)
    {
        // _endpoints.Add(new Endpoint(path, "GET", handler));
    }
    public void MapPost(string path, Func<RequestContext, string> handler)
    {
        _endpoints.Add(new Endpoint(path, "POST", handler));
    }
    public string Resolve(RequestContext context)
    {
        var endpoint = _endpoints.FirstOrDefault(ep => ep.Path == context.Path && ep.Method == context.Method);
        return endpoint != null 
            ? endpoint.Handler(context) 
            : "404 Not Found";
    }
}