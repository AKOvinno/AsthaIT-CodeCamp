public class RiderNotifier : IRideObserver
{
    private readonly string _riderName;
    private readonly string _riderPhone;

    public RiderNotifier(string riderName, string riderPhone)
    {
        _riderName = riderName;
        _riderPhone = riderPhone;
    }

    public void Update(string rideId, string status, string pricingStrategy)
    {
        Console.WriteLine($"[SMS to {_riderName} ({_riderPhone})] Ride {rideId} is now {status} | Pricing: {pricingStrategy}");
    }
}

