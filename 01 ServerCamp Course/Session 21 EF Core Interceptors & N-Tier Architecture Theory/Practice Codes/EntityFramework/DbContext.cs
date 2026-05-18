using Microsoft.EntityFrameworkCore;
// Here, AppDbContest representing the full-connection to the database
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    {
        
    }
    // DbContext provides a method called OnModelCreating, which is used to configure the model and its relationships. We are overriding this method to specify how the User entity should be mapped to the database table.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("APP_USERS");
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => new { e.Id});
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Email)
                .IsRequired()
                .HasColumnName("USER_EMAIL");
            entity.HasIndex(e => e.Email).IsUnique();
        });
        modelBuilder.Entity<User>()
            .HasOne(u => u.UserProfile)
            .WithOne(up => up.User)
            .HasForeignKey<UserProfile>(up => up.UserId); // In casse of one-to-one relationship, we need to specify the foreign key on the dependent entity, which is UserProfile in this case. This tells EF Core that the UserId property in UserProfile is the foreign key that links to the User entity. 
        
        modelBuilder.Entity<User>()
            .HasMany(u => u.UserOrders)
            .WithOne(o => o.User)
            .HasForeignKey(o => o.UserId);

        modelBuilder.Entity<Enrollment>()
            .HasKey(k => new { k.StudentId, k.CourseId }); // In case of many-to-many relationship, we need to specify a composite key on the pivot table (Enrollment) that consists of the foreign keys to both entities (StudentId and CourseId). This ensures that each combination of StudentId and CourseId is unique in the Enrollment table, preventing duplicate enrollments for the same student and course.

        base.OnModelCreating(modelBuilder);
    }
    // This is a DbSet representing the User table
    public DbSet<User> Users { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
}

