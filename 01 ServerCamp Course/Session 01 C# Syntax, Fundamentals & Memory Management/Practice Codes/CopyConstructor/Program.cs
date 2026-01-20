class Vehicle {

    // variables
    private string name;
    private string color;
    private int quantity;

    //  Copy constructor
    public Vehicle(Vehicle a)
    {
        name = a.name;
        color = a.color;
        quantity = a.quantity;
    }

    // Parameterized constructor
    public Vehicle(string name, string color, int quantity)
    {
        this.name = name;
        this.color = color;
        this.quantity = quantity;
    }

    // Get details of Vehicles
    public string DetailsOfVehicle
    {
        get
        {
            return "Type: " + name.ToString() + 
                   "\nColor: " + color.ToString() + 
                   "\nQuantity: " + quantity.ToString();
        }
    }

    // Main Method
    public static void Main()
    {

        // Create a new object.
        Vehicle v1 = new Vehicle("Bike", "Black", 40);

        // here is v1 details are copied to v2 using copy constructor
        Vehicle v2 = new Vehicle(v1);
        v2.color = "Yellow"; // changing color of v2
        Console.WriteLine("V2 Vehicle:\n"+ v2.DetailsOfVehicle);

        // v1 details remains unchanged because v2 is a copy of v1
        Console.WriteLine("\nV1 Vehicle:\n"+ v1.DetailsOfVehicle);

        // here v1 reference is assigned to v3 without using copy constructor
        Vehicle v3 = v1; // v3 is reference to v1
        v3.name = "Truck"; // changing color of v3 also changes color of v1
        Console.WriteLine("\nV1 Vehicle:\n"+ v1.DetailsOfVehicle);
    }
}