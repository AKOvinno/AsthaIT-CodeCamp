Console.WriteLine("========================================");
Console.WriteLine("     WELCOME TO RIDE SHARING SYSTEM     ");
Console.WriteLine("========================================");

RideManager rideManager = RideManager.GetInstance();
List<Rider> riders = new List<Rider>();
List<Ride> rides = new List<Ride>();
int rideCounter = 1;

bool running = true;
while (running)
{
    Console.WriteLine("\n--- MAIN MENU ---");
    Console.WriteLine("1. Register Driver");
    Console.WriteLine("2. Register Rider");
    Console.WriteLine("3. View All Drivers");
    Console.WriteLine("4. View All Riders");
    Console.WriteLine("5. View Available Drivers by Vehicle Type");
    Console.WriteLine("6. Create Ride");
    Console.WriteLine("7. View All Rides");
    Console.WriteLine("8. Update Ride Status");
    Console.WriteLine("9. Process Payment");
    Console.WriteLine("0. Exit");
    Console.Write("\nSelect option: ");

    string choice = Console.ReadLine() ?? "";

    switch (choice)
    {
        case "1":
            RegisterDriver(rideManager);
            break;
        case "2":
            riders.Add(RegisterRider());
            break;
        case "3":
            ViewAllDrivers(rideManager);
            break;
        case "4":
            ViewAllRiders(riders);
            break;
        case "5":
            ViewAvailableDrivers(rideManager);
            break;
        case "6":
            CreateRide(rideManager, riders, rides, ref rideCounter);
            break;
        case "7":
            ViewAllRides(rides);
            break;
        case "8":
            UpdateRideStatus(rides);
            break;
        case "9":
            ProcessPayment(rides);
            break;
        case "0":
            running = false;
            Console.WriteLine("\nThank you for using Ride Sharing System!");
            break;
        default:
            Console.WriteLine("Invalid option. Please try again.");
            break;
    }
}


static void RegisterDriver(RideManager rideManager)
{
    Console.Write("\nEnter Driver ID: ");
    string id = Console.ReadLine() ?? "";
    Console.Write("Enter Driver Name: ");
    string name = Console.ReadLine() ?? "";
    Console.Write("Enter Phone Number: ");
    string phone = Console.ReadLine() ?? "";
    Console.Write("Enter Vehicle Type (Bike/CNG/Car): ");
    string vehicleType = Console.ReadLine() ?? "";

    try
    {
        IVehicle vehicle = VehicleFactory.CreateVehicle(vehicleType);
        Driver driver = new Driver(id, name, phone, vehicle);
        rideManager.RegisterDriver(driver);
        driver.DisplayInfo();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}

static Rider RegisterRider()
{
    Console.Write("\nEnter Rider ID: ");
    string id = Console.ReadLine() ?? "";
    Console.Write("Enter Rider Name: ");
    string name = Console.ReadLine() ?? "";
    Console.Write("Enter Phone Number: ");
    string phone = Console.ReadLine() ?? "";
    Rider rider = new Rider(id, name, phone);
    Console.WriteLine($"Rider {rider.Name} registered successfully.");
    rider.DisplayInfo();
    return rider;
}

static void ViewAllDrivers(RideManager rideManager)
{
    Console.WriteLine("\n--- ALL DRIVERS ---");
    List<Driver> drivers = rideManager.GetAllDrivers();
    
    if (drivers.Count == 0)
    {
        Console.WriteLine("No drivers registered.");
        return;
    }

    foreach (Driver driver in drivers)
    {
        driver.DisplayInfo();
    }
}

static void ViewAllRiders(List<Rider> riders)
{
    Console.WriteLine("\n--- ALL RIDERS ---");

    if (riders.Count == 0)
    {
        Console.WriteLine("No riders registered.");
        return;
    }

    foreach (Rider rider in riders)
    {
        rider.DisplayInfo();
    }
}

static void ViewAvailableDrivers(RideManager rideManager)
{
    Console.Write("\nEnter Vehicle Type (Bike/CNG/Car): ");
    string vehicleType = Console.ReadLine() ?? "";

    Console.WriteLine($"\n--- AVAILABLE {vehicleType.ToUpper()} DRIVERS ---");
    List<Driver> drivers = rideManager.GetAvailableDrivers(vehicleType);
    
    if (drivers.Count == 0)
    {
        Console.WriteLine($"No available {vehicleType} drivers found.");
        return;
    }

    foreach (Driver driver in drivers)
    {
        driver.DisplayInfo();
    }
}

static void CreateRide(RideManager rideManager, List<Rider> riders, List<Ride> rides, ref int rideCounter)
{
    if (riders.Count == 0)
    {
        Console.WriteLine("\nNo riders registered. Please register a rider first.");
        return;
    }

    List<Driver> availableDrivers = rideManager.GetAllDrivers().Where(d => d.IsAvailable).ToList();
    if (availableDrivers.Count == 0)
    {
        Console.WriteLine("\nNo available drivers. Please register drivers first.");
        return;
    }

    Console.WriteLine("\n--- CREATE RIDE ---");
    Console.WriteLine("Select Rider:");
    for (int i = 0; i < riders.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {riders[i].Name}");
    }
    Console.Write("Choice: ");
    int riderChoice = int.TryParse(Console.ReadLine(), out int rc) ? rc - 1 : -1;

    if (riderChoice < 0 || riderChoice >= riders.Count)
    {
        Console.WriteLine("Invalid rider selection.");
        return;
    }

    Console.WriteLine("\nSelect Driver:");
    for (int i = 0; i < availableDrivers.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {availableDrivers[i].Name} ({availableDrivers[i].Vehicle.GetVehicleType()})");
    }
    Console.Write("Choice: ");
    int driverChoice = int.TryParse(Console.ReadLine(), out int dc) ? dc - 1 : -1;

    if (driverChoice < 0 || driverChoice >= availableDrivers.Count)
    {
        Console.WriteLine("Invalid driver selection.");
        return;
    }

    Console.Write("Enter Distance (km): ");
    double distance = double.TryParse(Console.ReadLine(), out double d) ? d : 0;

    // Automatically select pricing strategy based on current time
    IPricingStrategy strategy;
    int hour = DateTime.Now.Hour;
    if (hour >= 0 && hour < 6)
    {
        strategy = new MidnightPricing();
    }
    else if ((hour >= 7 && hour <= 9) || (hour >= 17 && hour <= 19))
    {
        strategy = new RushHourPricing();
    }
    else
    {
        strategy = new StandardPricing();
    }

    Ride ride = new Ride($"R{rideCounter++}", riders[riderChoice], availableDrivers[driverChoice], distance);
    ride.SetPricingStrategy(strategy);
    
    // Add observers
    ride.AddObserver(new RiderNotifier(riders[riderChoice].Name, riders[riderChoice].Phone));
    ride.AddObserver(new DriverNotifier(availableDrivers[driverChoice].Name));

    rides.Add(ride);
    availableDrivers[driverChoice].IsAvailable = false;

    // Notify observers immediately about the created ride (includes pricing info)
    ride.SetStatus(ride.Status);

    Console.WriteLine("\nRide created successfully!");
    ride.DisplayInfo();
}

static void ViewAllRides(List<Ride> rides)
{
    Console.WriteLine("\n--- ALL RIDES ---");
    
    if (rides.Count == 0)
    {
        Console.WriteLine("No rides created.");
        return;
    }

    foreach (Ride ride in rides)
    {
        ride.DisplayInfo();
    }
}

static void UpdateRideStatus(List<Ride> rides)
{
    if (rides.Count == 0)
    {
        Console.WriteLine("\nNo rides available.");
        return;
    }

    Console.WriteLine("\n--- UPDATE RIDE STATUS ---");
    Console.WriteLine("Select Ride:");
    for (int i = 0; i < rides.Count; i++)
    {
        Console.WriteLine($"{i + 1}. Ride {rides[i].Id} - Current Status: {rides[i].Status}");
    }
    Console.Write("Choice: ");
    int rideChoice = int.TryParse(Console.ReadLine(), out int rc) ? rc - 1 : -1;

    if (rideChoice < 0 || rideChoice >= rides.Count)
    {
        Console.WriteLine("Invalid ride selection.");
        return;
    }

    Console.WriteLine("\nSelect New Status:");
    Console.WriteLine("1. Requested");
    Console.WriteLine("2. Accepted");
    Console.WriteLine("3. In Progress");
    Console.WriteLine("4. Completed");
    Console.Write("Choice: ");
    string statusChoice = Console.ReadLine() ?? "";

    string newStatus;
    switch (statusChoice)
    {
        case "1":
            newStatus = "Requested";
            break;
        case "2":
            newStatus = "Accepted";
            break;
        case "3":
            newStatus = "In Progress";
            break;
        case "4":
            newStatus = "Completed";
            break;
        default:
            newStatus = rides[rideChoice].Status;
            break;
    }

    Console.WriteLine();
    rides[rideChoice].SetStatus(newStatus);
    Console.WriteLine($"\nRide {rides[rideChoice].Id} status updated to: {newStatus}");

    if (newStatus == "Completed")
    {
        rides[rideChoice].Driver.IsAvailable = true;
    }
}

static void ProcessPayment(List<Ride> rides)
{
    if (rides.Count == 0)
    {
        Console.WriteLine("\nNo rides available.");
        return;
    }

    Console.WriteLine("\n--- PROCESS PAYMENT ---");
    Console.WriteLine("Select Ride:");
    for (int i = 0; i < rides.Count; i++)
    {
        Console.WriteLine($"{i + 1}. Ride {rides[i].Id} - Fare: ${rides[i].CalculateFare():F2}");
    }
    Console.Write("Choice: ");
    int rideChoice = int.TryParse(Console.ReadLine(), out int rc) ? rc - 1 : -1;

    if (rideChoice < 0 || rideChoice >= rides.Count)
    {
        Console.WriteLine("Invalid ride selection.");
        return;
    }

    Console.WriteLine("\nSelect Payment Method:");
    Console.WriteLine("1. bKash");
    Console.WriteLine("2. Credit Card");
    Console.Write("Choice: ");
    string paymentChoice = Console.ReadLine() ?? "";

    IPaymentProcessor? processor;
    switch (paymentChoice)
    {
        case "1":
            processor = new BkashPaymentAdapter();
            break;
        case "2":
            processor = new CreditCardProcessor();
            break;
        default:
            processor = null;
            break;
    }

    if (processor == null)
    {
        Console.WriteLine("Invalid payment method.");
        return;
    }

    Console.Write($"Enter {processor.GetPaymentMethod()} Info: ");
    string paymentInfo = Console.ReadLine() ?? "";

    double amount = rides[rideChoice].CalculateFare();
    Console.WriteLine();
    bool success = processor.Pay(paymentInfo, amount);

    if (success)
    {
        Console.WriteLine($"\nPayment of ${amount:F2} processed successfully via {processor.GetPaymentMethod()}!");
    }
    else
    {
        Console.WriteLine($"\nPayment failed!");
    }
}


