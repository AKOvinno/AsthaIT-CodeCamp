using System;
public static class BusFactory // Factory Method Pattern
{
    public static Bus CreateBus(int busId, string coachNumber, string busType)
    {
        switch (busType.ToLower())
        {
            case "business":
                return new BusinessBus(busId, coachNumber);
            case "economy":
                return new EconomyBus(busId, coachNumber);
            default:
                throw new ArgumentException($"Invalid bus type: {busType}. Valid types are: Business, Economy");
        }
    }
}
