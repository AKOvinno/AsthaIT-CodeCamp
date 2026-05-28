using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "codecamp";

        options.Events.OnRedirectToAccessDenied = context =>
        {
            context.Response.StatusCode = 401;
            return Task.CompletedTask;
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();

// 1. Who are you?
app.UseAuthentication(); 

// 2. Are you allowed to see this? (Must always come AFTER Authentication)
app.UseAuthorization();

app.MapGet("/cookie-authorized", (HttpContext context) => 
{
    return Results.Ok("You're authenticated by Cookie");
}).RequireAuthorization();

app.MapGet("/login", async (string userName, string userPassword, HttpContext context) =>
{
    if(userName != "Ovinno" && userPassword != "password") return Results.Unauthorized();
    // generate secret
    var claims = new List<Claim>
    {
        new(type: "username", userName),
        new(type: "batch", "codecamp-3")
    };
    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    var claimsPrinciple = new ClaimsPrincipal(claimsIdentity);
    await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrinciple);
    return Results.Ok();
});

app.Run();

