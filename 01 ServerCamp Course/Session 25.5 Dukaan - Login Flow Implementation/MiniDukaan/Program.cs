using System.Text;
using Dukaan.Application.Interfaces;
using Dukaan.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MiniDukaan.Infrastructure.Data.DbContext;
using MiniDukaan.Infrastructure.Data.Model;
using MiniDukaan.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Step 1
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Register ASP.NET Core Identity for authentication
builder.Services.AddIdentity<Merchant, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Default authentication scheme and JWT authentication configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

// We are injecting dependency in Scoped because we want a request will enter and that request's scoped only one repository/tenant service should be created.
builder.Services.AddScoped<TenantService>();
builder.Services.AddScoped(typeof(Repository<>));

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddOpenApi();

builder.Services.AddControllers();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Reads the incoming request, validates the authentication token (like JWT or cookie), and sets the user identity
app.UseAuthentication();

app.MapControllers();

app.Run();

