Rider rider = new Rider("John Doe", 0, "123-456-7890", "jdoe@rd.com", "123 Main St, New York, USA", "4.5", "ABC123", "XYZ123");
Passenger passenger = new Passenger("Jane Smith", 0, "987-654-3210", "jsmith@rd.com", "456 Oak Ave, Arizona, USA", "4.8", "O+");
Rider rider3 = new Rider("John Doe", 0, "123-456-7890", "jdoe@rd.com", "123 Main St, New York, USA", "4.5", "ABC123", "XYZ123");

rider.DisplayInfo();
passenger.DisplayInfo();

// rider and rider3 have same data but are different instances
// so they are not equal because class uses reference equality
Console.WriteLine($"\nAre rider and rider3 equal? {rider == rider3}");

Cat cat1 = new Cat { Name = "Whiskers", Age = 3 };
Cat cat2 = new Cat { Name = "Whiskers", Age = 3 };
// cat1 and cat2 have same data and are different instances
// so they are equal because record struct uses value equality
Console.WriteLine($"\nAre cat1 and cat2 equal? {cat1 == cat2}");
// If we want to use record struct then we have to sacrifice inheritance
// record struct cannot inherit from another class or struct


// abstract class cannot be instantiated
// var person = new Person() ; will give error

// Domain Classes: Rider, Passenger, Payment
// Person class demonstrate IS-A relationship with Rider and Passenger
// Person is abstract because we don't want to create Person objects directly
//
abstract class Person
{
    public Person(string name, int id, string mobileNo, string email, string currentLocation, string rating)
    {
        ID = GenerateUniqueID();
        Name = name;
        MobileNo = mobileNo;
        Email = email;
        CurrentLocation = currentLocation;
        Rating = rating;
    }
    // Fields or properties
    public string Name { get; protected set; }
    // init only be set during object construction
    // readonly after that
    protected int ID { get; init; }
    protected string MobileNo { get; set; }
    protected string Email { get; set; }
    protected string CurrentLocation { get; set; } // TODO lat lon
    protected string Rating { get; set; }

    public int GenerateUniqueID()
    {
        Random random = new Random();
        return random.Next(1000, 9999); // Generates a random ID between 1000 and 9999
    }
}
// Rider & Passenger class demonstrate IS-A relationship with Person
class Rider : Person
{
    // Fields or properties
    // access_modifier data_type field_name
    // access_modifier data_type Property_name {get; set;}
    public Rider(string name, int id, string mobileNo, string email, string currentLocation, string rating, string vehicleRegistrationNo, string driverLicenseNo) : base(name, id, mobileNo, email, currentLocation, rating)
    {
        VehicleRegistrationNo = vehicleRegistrationNo;
        DriverLicenseNo = driverLicenseNo;
    }
    public string VehicleRegistrationNo { get; set; }
    public string DriverLicenseNo { get; set; }

    // methods
    public void DisplayInfo()
    {
    Console.WriteLine("\n--- Rider Info ---");
    Console.WriteLine("Name: " + Name);
    Console.WriteLine("ID: " + ID);
    Console.WriteLine("Mobile No: " + MobileNo);
    Console.WriteLine("Email: " + Email);
    Console.WriteLine("Current Location: " + CurrentLocation);
    Console.WriteLine("Rating: " + Rating);
    Console.WriteLine("Vehicle Registration No: " + VehicleRegistrationNo);
    Console.WriteLine("Driver License No: " + DriverLicenseNo);
    }
}
class Passenger : Person
{
    public Passenger(string name, int id, string mobileNo, string email, string currentLocation, string rating, string bloodGroup) : base(name, id, mobileNo, email, currentLocation, rating)
    {
        BloodGroup = bloodGroup;
    }
    // Fields or properties
    public string BloodGroup { get; set; }
    public void DisplayInfo()
    {
    Console.WriteLine("\n--- Passenger Info ---");
    Console.WriteLine("Name: " + Name);
    Console.WriteLine("ID: " + ID);
    Console.WriteLine("Mobile No: " + MobileNo);
    Console.WriteLine("Email: " + Email);
    Console.WriteLine("Current Location: " + CurrentLocation);
    Console.WriteLine("Rating: " + Rating);
    Console.WriteLine("Blood Group: " + BloodGroup);
    }
}
class Payment
{
    // In Payment we need to have both Rider and Passenger information
    // So we cannot inherit from Rider and Passenger
    // Instead we can have Rider and Passenger as properties
    // This way Payment HAS-A Rider and HAS-A Passenger
    // This is composition relationship or HAS-A relationship
    // It's also called composition over inheritance
    public Payment(Rider rider, Passenger passenger, double amount, string paymentMethod)
    {
        Rider = rider;
        Passenger = passenger;
        Amount = amount;
        PaymentMethod = paymentMethod;
    }
    // fields or properties

    // Initially we can have RiderID and PassengerID
    // We don't need to have full Rider and Passenger objects here
    // But later we can have full objects or it's other properties for more complex operations
    
    
    // Here we are not inheriting Rider or Passenger
    // We are just using them as properties
    // This is composition relationship or HAS-A relationship
    // It's also called composition over inheritance
    // It's also called aggregation relationship because Payment can exist without Rider or Passenger
    public Rider Rider { get; set; }
    public int RiderID {get; set;}
    public Passenger Passenger { get; set; }
    public int PassengerID {get; set;}
    public double Amount { get; set; }
    public string PaymentMethod { get; set; }

    public void DisplayPaymentInfo()
    {
        Console.WriteLine("\n--- Payment Info ---");
        Console.WriteLine("Rider Name: " + Rider.Name);
        Console.WriteLine("Passenger Name: " + Passenger.Name);
        Console.WriteLine("Amount: " + Amount);
        Console.WriteLine("Payment Method: " + PaymentMethod);
    }
}

// Helper or Manager Classes
// It's not an entity or value object it's a workflow or process
// Responsible for operations related to Payment
class PaymentManager // HAS A Payment
{
    // Instead of payment two times we can have it once as a field or property
    public Payment payment {get; set;} // We can use composition if PaymentManager only need to work with one Payment like bKash all the time

    // In case of multiple payments we can have it as method parameters / method injection
    // public void ProcessPayment(Payment payment) { }
    public PaymentManager(Payment payment)
    {
        this.payment = payment;
    }
    public void ProcessPayment(/*Payment payment*/)
    {
        Console.WriteLine($"\nProcessing payment of {payment.Amount} via {payment.PaymentMethod}...");
        // Simulate payment processing logic
        Console.WriteLine("Payment processed successfully.");
    }
    public bool ValidatePayment(/*Payment payment*/) 
    {
        // Simulate payment validation logic
        Console.WriteLine("Validating payment...");
        return true; // Assume payment is always valid for this example
    }
}
record struct Cat
{
    public Cat(string name, int age)
    {
        Name = name;
        Age = age;
    } 
    public string Name { get; set; }
    public int Age { get; set; }
}

// Example to prove inheritance doesn't work always
// Composition is the better approach in such cases
class Human
{
    string Name;
    string Roll;
}
class Teacher : Human
{
    // public void StaysLateInSchool()
    // {
    //     Console.WriteLine("Staying late in school");
    // }
    // Instead of inheritance we can use composition here
    public StayLateManager stayLateManager = new StayLateManager();
    // If want to access stayLateManager method in composite class
    // we can create a method in composite class that calls the method in stayLateManager
    public void StaysLateInSchool()
    {
        stayLateManager.StaysLateInSchool();
    }
}
class Student : Human
{
    
}
class BlackStudent : Student
{
    // public void StaysLateInSchool()
    // {
    //     Console.WriteLine("Staying late in school");
    // }
    // Instead of inheritance we can use composition here
    public StayLateManager stayLateManager = new StayLateManager();
}
class WhiteStudent : Student
{
    
}

// Here code is duplicating because both Teacher and BlackStudent have staysLateInSchool method
// Inheritance is not working properly here
// So we can use composition instead of inheritance
// We will create a helper class StayLateManager
// and use it in both Teacher and BlackStudent classes
// This way we avoid code duplication and make the code more maintainable
class StayLateManager
{
    public void StaysLateInSchool()
    {
        Console.WriteLine("Staying late in school");
    } 
}
