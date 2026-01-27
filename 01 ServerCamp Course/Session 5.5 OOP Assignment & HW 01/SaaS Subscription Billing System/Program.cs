Console.WriteLine("SaaS Subscription Billing System");
// Additional code for the billing system would go here
class User
{
    public User()
    {
        Id = Guid.NewGuid();
    }
    public User(string name, string email, Subscription subscription, string invoices) : this()
    {
        Name = name;
        Email = email;
        Subscription = subscription;
        Invoices = invoices;
    }
    public Guid Id { get; private set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public Subscription Subscription { get; set; }
    public string Invoices { get; set; }

    public void DisplayInfo()
    {
        Console.WriteLine($"User ID: {Id}");
        Console.WriteLine($"Name: {Name}");
        Console.WriteLine($"Email: {Email}");
        Console.WriteLine($"Subscription Plan: {Subscription.PlanName}");
        Console.WriteLine($"Subscription Price: {Subscription.Price}");
        Console.WriteLine($"Subscription Start Date: {Subscription.StartDate}");
        Console.WriteLine($"Subscription Expiry Date: {Subscription.ExpiryDate}");
        Console.WriteLine($"Is Active: {Subscription.IsActive}");
        Console.WriteLine($"Bill Paid: {Subscription.BillPaid}");
        Console.WriteLine($"Notification Channel: {Subscription.NotificationChannel}");
        Console.WriteLine($"Invoices: {Invoices}");
    }

}
class Subscription : User
{
    public string PlanName { get; set; }
    public double Price { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public bool IsActive { get; set; }
    public bool BillPaid { get; set; }
    public string NotificationChannel { get; set; }
}