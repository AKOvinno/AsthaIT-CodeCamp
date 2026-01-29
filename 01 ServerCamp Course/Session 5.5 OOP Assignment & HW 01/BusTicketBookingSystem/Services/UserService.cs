using System;
using System.Collections.Generic;
using System.Linq;

public interface IUserService
{
    void CreateUser(string name, string mobileNumber, string email);
    List<User> GetAllUsers();
    User GetUserById(int id);
}

public class UserService : IUserService
{
    private readonly List<User> _users;

    public UserService()
    {
        _users = new List<User>();
    }

    public void CreateUser(string name, string mobileNumber, string email)
    {
        // Validation
        if (ValidationHelper.IsNullOrEmpty(name))
            throw new ArgumentException("Name cannot be empty");
        if (ValidationHelper.IsNullOrEmpty(mobileNumber))
            throw new ArgumentException("Mobile number cannot be empty");
        if (ValidationHelper.IsNullOrEmpty(email))
            throw new ArgumentException("Email cannot be empty");

        // Create user
        int userId = IdGenerator.GetNextUserId();
        User user = new User(userId, name, mobileNumber, email);
        _users.Add(user);

        Console.WriteLine($"User created successfully! User ID: {userId}");
    }

    public List<User> GetAllUsers()
    {
        return _users.ToList(); // Return a copy of the list to prevent external modification of the original list. Doesn't break encapsulation.
    }
    public User GetUserById(int id)
    {
        // .FirstOrDefault is a LINQ method that returns the first element of a sequence that satisfies a specified condition or a default value if no such element is found.
        return _users.FirstOrDefault(u => u.UserId == id);
    }
}
