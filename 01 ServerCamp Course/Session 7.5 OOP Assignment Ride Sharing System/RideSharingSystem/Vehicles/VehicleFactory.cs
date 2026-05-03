public class VehicleFactory
{
    public static IVehicle CreateVehicle(string type)
    {
        if(String.Equals(type.ToLower(), "bike")) return new Bike();
        if(String.Equals(type.ToLower(), "cng")) return new CNG();
        if(String.Equals(type.ToLower(), "car")) return new Car();
        throw new ArgumentException($"Invalid vehicle type: {type}");
    }
}
