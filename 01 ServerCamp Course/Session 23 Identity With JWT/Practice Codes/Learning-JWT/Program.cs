using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

app.Services.AddAuthentication();

var app = builder.Build();

var key = "YOUR_SUPER_SECRET_KEY_THAT_IS_LONG_ENOUGH_256_BITS"; // This should be stored securely, e.g., in environment variables or a secure vault



app.MapGet("/login-with-jwt", (string userName, string password, HttpContext context) =>
{
    if(userName != "ovinno" && password != "password")
        return Results.Unauthorized();

    var claims = new List<Claim>
    {
        new(ClaimTypes.Name, userName),
        new(ClaimTypes.Role, "admin")
    };
    var tokenDescriptor = new SecurityTokenDescriptor()
    {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.UtcNow.AddMinutes(30),
        SigningCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256Signature
        )
    };
    
    var handler = new JwtSecurityTokenHandler();
    var token = handler.CreateToken(tokenDescriptor);
    var jwt = handler.WriteToken(token);

    return Results.Ok(new { token = jwt });
});

app.Run();
