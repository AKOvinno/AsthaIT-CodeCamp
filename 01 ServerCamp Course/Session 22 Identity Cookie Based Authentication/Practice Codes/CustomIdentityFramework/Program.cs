using System.Security.Claims;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.Use(async (context, next) =>
{
    // This three lines is the summary of below full code
    // var handlers = getHandlers();
    // var cookieHandler = handlers.Find("cookie-handler");
    // cookieHandler.handler();

    var authCookie = context.Request.Headers.Cookie.FirstOrDefault(c => c.StartsWith("codecamp"));
    if(authCookie == null || authCookie.Length <= 0)
    {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("Unauthorized");
        return;
    }
    var payload = authCookie.Split("=").Last(); // codecamp=username:ovinno
    var parts = payload.Split(":");
    var key = parts[0];
    var value = parts[1];

    var claims = new List<Claim>
    {
        new(key, value)
    };
    var claimsIdentity = new ClaimsIdentity(claims);
    context.User = new ClaimsPrincipal(claimsIdentity);

    await next();
});

app.MapGet("/cookie-authorized", (HttpContext context) =>
{
    var valueObject = context.User.FindFirst("username");
    return Results.Ok(valueObject.Value);
});

// In this endpoint we are explicitly writing cookie
app.MapGet("/login", (string userName, string password, HttpContext context) =>
{
   if(userName != "ovinno" || password != "password") return Results.Unauthorized();

    var secret = $"username:{userName}";
    context.Response.Headers["set-cookie"] = $"codecamp={secret}";
    return Results.Ok();

//    AuthFactory.SignIn("bearer");
//    return Results.Ok();
});


app.Run();


public class AuthFactory
{
    public static IIAuthService SignIn(string scheme)
    {
        if(scheme == "cookie") return new CookieAuthServicee();
        if(scheme == "bearer") return new BearerAuthServicee();

        return null;
    }
}
public interface IIAuthService
{
    public Task SignIn();
}
public class CookieAuthServicee : IIAuthService
{
    public Task SignIn()
    {
        Console.WriteLine("Sign in with Cookie");
        return Task.CompletedTask;
        // var secret = $"username:{userName}";
        // context.Response.Headers["set-cookie"] = $"codecamp={secret}";
        // return Results.Ok();
    }
}
public class BearerAuthServicee : IIAuthService
{
    public Task SignIn()
    {
        Console.WriteLine("Sign in with Bearer");
        return Task.CompletedTask;
        // var secret = $"username:{userName}";
        // context.Response.Headers["set-cookie"] = $"codecamp={secret}";
        // return Results.Ok();
    }
}