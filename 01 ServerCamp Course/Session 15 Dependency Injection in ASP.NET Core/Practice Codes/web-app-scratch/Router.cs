public class Router
{
    private readonly List<Endpoint> _endpoints = []; // In real implementation, this would be a collection to store registered endpoints and their handlers but here we are just defining the structure of the Router class and its method for mapping GET requests.
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
        // Here, FirstOrDefault is used to find the first endpoint in the _endpoints list that matches the requested path and method. If a matching endpoint is found, its handler is invoked with the request context to generate the response. If no matching endpoint is found, a "404 Not Found" response is returned.
        var endpoint = _endpoints.FirstOrDefault(ep => ep.Path == context.Path && ep.Method == context.Method);
        return endpoint != null 
            ? endpoint.Handler(context) 
            : "404 Not Found";
    }
}