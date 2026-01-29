public class User
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public string MobileNumber { get; set; }
    public string Email { get; set; }

    public User(int userId, string name, string mobileNumber, string email)
    {
        UserId = userId;
        Name = name;
        MobileNumber = mobileNumber;
        Email = email;
    }
    public override string ToString()
    {
        return "User ID: " + UserId + "\nName: " + Name + "\nMobile: " + MobileNumber + "\nEmail: " + Email;
    }
}
