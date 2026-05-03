public class Driver : User
{
    public IVehicle Vehicle { get; set; }
    public bool IsAvailable { get; set; }

    public Driver(string id, string name, string phone, IVehicle vehicle, bool isAvailable = true)
        : base(id, name, phone)
    {
        Vehicle = vehicle;
        IsAvailable = isAvailable;
    }

    public override void DisplayInfo()
    {
        string status = IsAvailable ? "Available" : "Busy";
        Console.WriteLine($"[Driver] ID: {Id}, Name: {Name}, Phone: {Phone}, Vehicle: {Vehicle.GetVehicleType()}, Status: {status}");
    }

    public override string GetRole() => "Driver";
}
