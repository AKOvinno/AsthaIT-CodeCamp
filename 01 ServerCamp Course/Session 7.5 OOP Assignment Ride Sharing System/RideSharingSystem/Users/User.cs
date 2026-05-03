public abstract class User
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }

    protected User(string id, string name, string phone)
    {
        Id = id;
        Name = name;
        Phone = phone;
    }

    public abstract void DisplayInfo();
    public abstract string GetRole();
}
