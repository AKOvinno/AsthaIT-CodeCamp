using Microsoft.EntityFrameworkCore;
// Here, AppDbContest representing the full-connection to the database
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    {
        
    }
    public DbSet<User> Users { get; set; }
}

