using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens; // dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql("Host=localhost;Port=5432;Database=identity_db;Username=postgres;Password=root");
});

// We need these dependency injections for registration and other services
builder.Services
    .AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 1;
        options.Password.RequiredUniqueChars = 0;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

var key = "a-very-long-and-secure-secret-key-at-least-32-chars"u8.ToArray();
// Injecting the JWT authentication service into the dependency injection container
builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "ovinno",
            ValidAudience = "ovinno",
            IssuerSigningKey = new SymmetricSecurityKey(("a-very-long-and-secure-secret-key-at-least-32-chars"u8.ToArray()))
        };
    });

// Now we have to call authorization which is gonna validate claim
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("admin-policy", policy =>
        {
            policy.RequireClaim("org", "ait");
            policy.RequireRole("admin");
        }
    );
});

var app = builder.Build();

app.UseAuthentication(); // Use Authentication generates claims
app.UseAuthorization();

app.MapPost("/register", async (
    string email,
    string password,
    UserManager<IdentityUser> userManager) =>
{
    var user = new IdentityUser
    {
      UserName = email.Split('@')[0],
      Email = email
    };
    var result = await userManager.CreateAsync(user, password);
    if(!result.Succeeded)
        return Results.BadRequest("User Creation Failed");
    return Results.Ok($"User {user.UserName} created successfully");
});

app.MapGet("/login", async (
    string email, 
    string password, 
    UserManager<IdentityUser> userManager) =>
{
    if(string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        return Results.Unauthorized();
        
    var user = await userManager.FindByEmailAsync(email);
    if(user is null)
        return Results.Unauthorized();

    var isPasswordValid = await userManager.CheckPasswordAsync(user, password);
    if(!isPasswordValid) return Results.Unauthorized();

    var organization = email switch
    {
        _ when email.EndsWith("@ait.com") => "ait",
        _ when email.EndsWith("@optimizely.com") => "optimizely",
        _ when email.EndsWith("@fieldnation.com") => "fieldnation",
        _ => "unknown"
        // here, (_) is called discard pattern in C#. It acts as a "catch-all" or "default" case
    };
    var role = email switch
    {
        _ when email.EndsWith("ovinno@ait.com") => "admin",
        _ => "user"
    };
    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(
        [
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim("org", organization),
            new Claim(ClaimTypes.Role, role),
        ]),
        Expires = DateTime.UtcNow.AddMinutes(30),
        Issuer = "ovinno",
        Audience = "ovinno",
        SigningCredentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature
        )
    };
    var tokenHandler = new JwtSecurityTokenHandler();
    var token = tokenHandler.CreateToken(tokenDescriptor);
    var jwtToken = tokenHandler.WriteToken(token);

    return Results.Ok(new {token = jwtToken});
});
app.MapGet("/ait-resources", (HttpContext context) =>
{
    return Results.Ok("You accessed AIT Resources!");
}).RequireAuthorization(policy =>
{
    policy.RequireClaim("org", "ait");
});
app.MapGet("/ait-admin-resources", (HttpContext context) =>
{
    return Results.Ok("You accessed AIT Admin Resources!");
}).RequireAuthorization("admin-policy");
app.MapGet("/ait-partial-ceo-resources", (HttpContext context) =>
{
    return Results.Ok("You accessed AIT Admin Resources!");
}).RequireAuthorization("admin-policy");
app.MapGet("/optimizely-resources", (HttpContext context) =>
{
    return Results.Ok("You accessed Optimizely Resources!");
}).RequireAuthorization(policy =>
{
    policy.RequireClaim("org", "optimizely");
});
app.MapGet("/fieldnation-resources", (HttpContext context) =>
{
    return Results.Ok("You accessed Field Nation Resources!");
}).RequireAuthorization(policy =>
{
    policy.RequireClaim("org", "fieldnation");
});
app.Run();
