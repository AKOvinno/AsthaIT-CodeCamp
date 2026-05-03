public class DriverNotifier : IRideObserver
{
    private readonly string _driverName;

    public DriverNotifier(string driverName)
    {
        _driverName = driverName;
    }

    public void Update(string rideId, string status, string pricingStrategy)
    {
        Console.WriteLine($"[App notification to {_driverName}] Ride {rideId} is now {status} | Pricing: {pricingStrategy}");
    }
}
