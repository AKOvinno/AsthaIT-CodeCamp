using Microsoft.EntityFrameworkCore;
// Here, AppDbContest representing the full-connection to the database
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("APP_USERS");
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.Name });
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Email)
                .IsRequired()
                .HasColumnName("USER_EMAIL");
            entity.HasIndex(e => e.Email).IsUnique();
        });
        base.OnModelCreating(modelBuilder);
    }
    // This is a DbSet representing the User table
    public DbSet<User> Users { get; set; }
}

