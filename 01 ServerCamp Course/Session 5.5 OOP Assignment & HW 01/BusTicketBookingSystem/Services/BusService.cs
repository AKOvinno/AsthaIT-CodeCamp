using System;
using System.Collections.Generic;
using System.Linq;
public interface IBusService
{
    void CreateBus(string coachNumber, string busType);
    List<Bus> GetAllBuses();
    Bus GetBusById(int id);
}
public class BusService : IBusService
{
    private readonly List<Bus> _buses;

    public BusService()
    {
        _buses = new List<Bus>();
    }

    public void CreateBus(string coachNumber, string busType)
    {
        // Validation
        if (ValidationHelper.IsNullOrEmpty(coachNumber))
            throw new ArgumentException("Coach number cannot be empty");

        if (ValidationHelper.IsNullOrEmpty(busType))
            throw new ArgumentException("Bus type cannot be empty");

        // Create bus using factory
        int busId = IdGenerator.GetNextBusId();
        Bus bus = BusFactory.CreateBus(busId, coachNumber, busType);
        _buses.Add(bus);

        Console.WriteLine("Bus created successfully!");
        Console.WriteLine($"Bus ID: {bus.BusId}");
        Console.WriteLine($"Coach Number: {bus.CoachNumber}");
        Console.WriteLine($"Bus Type: {bus.BusType}");
    }
    public List<Bus> GetAllBuses()
    {
        return _buses.ToList();
    }

    public Bus GetBusById(int id)
    {
        return _buses.FirstOrDefault(b => b.BusId == id);
    }
}
