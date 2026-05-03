public class Ride
{
    public string Id { get; set; }
    public Rider Rider { get; set; }
    public Driver Driver { get; set; }
    public double Distance { get; set; }
    public string Status { get; private set; }
    
    private IPricingStrategy? _pricingStrategy;
    private readonly List<IRideObserver> _observers;

    public Ride(string id, Rider rider, Driver driver, double distance)
    {
        Id = id;
        Rider = rider;
        Driver = driver;
        Distance = distance;
        Status = "Requested";
        _observers = new List<IRideObserver>();
    }

    public void AddObserver(IRideObserver observer)
    {
        _observers.Add(observer);
    }

    public void SetPricingStrategy(IPricingStrategy strategy)
    {
        _pricingStrategy = strategy;
    }

    public double CalculateFare()
    {
        if (_pricingStrategy == null)
        {
            throw new InvalidOperationException("Pricing strategy not set");
        }

        double baseFare = Driver.Vehicle.GetBaseFare();
        return _pricingStrategy.CalculateFare(Distance, baseFare);
    }

    public void SetStatus(string newStatus)
    {
        Status = newStatus;
        NotifyObservers();
    }

    private void NotifyObservers()
    {
        string pricingName = _pricingStrategy != null ? _pricingStrategy.GetStrategyName() : "Unknown Pricing";
        foreach (IRideObserver observer in _observers)
        {
            observer.Update(Id, Status, pricingName);
        }
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"\nRide ID: {Id}");
        Console.WriteLine($"Rider: {Rider.Name} | Driver: {Driver.Name}");
        Console.WriteLine($"Vehicle: {Driver.Vehicle.GetVehicleType()} | Distance: {Distance} km");
        Console.WriteLine($"Status: {Status}");
        
        if (_pricingStrategy != null)
        {
            Console.WriteLine($"Pricing: {_pricingStrategy.GetStrategyName()} | Fare: ${CalculateFare():F2}");
        }
    }
}
