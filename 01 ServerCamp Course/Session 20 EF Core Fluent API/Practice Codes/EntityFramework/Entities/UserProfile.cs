public class UserProfile
{
    public Guid Id { get; set; }
    public string Address { get; set; }
    public Guid UserId { get; set; } // This is the foreign key that will link to the User entity. If UserProfile is the name of the class and User is the name of the related class, then EF Core will automatically recognize UserId as the foreign key for the relationship.
}