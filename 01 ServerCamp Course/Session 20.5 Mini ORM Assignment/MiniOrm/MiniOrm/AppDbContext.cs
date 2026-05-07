using MiniOrm.Data;
using MiniOrm.Models;

namespace MiniOrm;

public class AppDbContext : DbContext
{
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;

    public AppDbContext(string connStr) : base(connStr) { }
}
