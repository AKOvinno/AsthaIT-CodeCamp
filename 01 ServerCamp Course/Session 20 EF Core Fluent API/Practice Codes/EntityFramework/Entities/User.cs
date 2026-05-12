using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public UserProfile UserProfile { get; set; } // Here, we are defining a navigation property to UserProfile. This is a one-to-one relationship, where each User has one UserProfile and each UserProfile is associated with one User.
}