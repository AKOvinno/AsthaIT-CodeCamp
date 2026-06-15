using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiniDukaan.Infrastructure.Data.DbContext;
using MiniDukaan.Infrastructure.Data.Model;
using MiniDukaan.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Step 1
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<Merchant, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// We are injecting dependency in Scoped because we want a request will enter and that request's scoped only one repository/tenant service should be created.
builder.Services.AddScoped<TenantService>();
builder.Services.AddScoped(typeof(Repository<>));
builder.Services.AddOpenApi();

builder.Services.AddControllers();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

