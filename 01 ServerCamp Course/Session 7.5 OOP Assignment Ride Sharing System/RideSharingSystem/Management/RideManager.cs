public class RideManager
{
    private static RideManager? _instance;
    private static readonly object _lockObject = new object();
    private readonly List<Driver> _drivers;

    private RideManager()
    {
        _drivers = new List<Driver>();
    }

    public static RideManager GetInstance()
    {
        if (_instance == null)
        {
            lock (_lockObject)
            {
                if (_instance == null)
                {
                    _instance = new RideManager();
                }
            }
        }
        return _instance;
    }

    public void RegisterDriver(Driver driver)
    {
        _drivers.Add(driver);
        Console.WriteLine($"Driver {driver.Name} registered successfully.");
    }

    public List<Driver> GetAllDrivers()
    {
        return _drivers;
    }

    public List<Driver> GetAvailableDrivers(string vehicleType)
    {
        return _drivers
            .Where(d => d.IsAvailable && d.Vehicle.GetVehicleType().Equals(vehicleType, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}
