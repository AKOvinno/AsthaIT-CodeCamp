public class Rider : User
{
    public double WalletBalance { get; set; }
    public Rider(string id, string name, string phone)
        : base(id, name, phone)
    {
        
    }
    
    public override void DisplayInfo()
    {
        Console.WriteLine($"[Rider] ID: {Id}, Name: {Name}, Phone: {Phone}");
    }

    public override string GetRole() => "Rider";
}
