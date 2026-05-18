public class UserProfile
{
    public Guid Id { get; set; }
    public string? Address { get; set; }
    public Guid UserId { get; set; } // This is the foreign key that will link to the User entity. If UserProfile is the name of the class and User is the name of the related class, then EF Core will automatically recognize UserId as the foreign key for the relationship.
    public User? User { get; set; } // This is the navigation property that allows you to access the related User entity from a UserProfile instance. It represents the one-to-one relationship between UserProfile and User.

    // We don't need Fluent API configurations for this relationship because EF Core can infer the relationship based on the naming conventions and the presence of the UserId foreign key and the User navigation property. However, if you want to explicitly configure the relationship using Fluent API, you can do so in your DbContext's OnModelCreating method.
}