// Violation of DIP
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

var services = new ServiceCollection();
services.AddTransient<PaymentService>(); // If anyone wants PaymentService, give them a PaymentService
services.AddTransient<INotificationService, EmailService>(); // If anyone wants INotificationService, give them an EmailService

// Now we build the service provider, which will allow us to resolve our dependencies. It will look at the services we registered and figure out how to create instances of our classes when we ask for them. It also handles the lifetime of our services. 
var serviceProvider = services.BuildServiceProvider();
var paymentService = serviceProvider.GetRequiredService<PaymentService>();
paymentService.Process();
// In this example, we have a PaymentService that depends on an INotificationService. We register both the PaymentService and the INotificationService (with its implementation, EmailService) in the service collection. When we build the service provider and request a PaymentService, it will automatically resolve the INotificationService dependency and also inject it into the PaymentService constructor. This way, we can easily swap out the INotificationService implementation without changing the PaymentService code, adhering to the Dependency Inversion Principle (DIP).
public class PaymentService
{
    // Here, I don't need to know which service is being used to send notifications. I just know that I have an INotificationService, and I can call the Send method on it. This allows for greater flexibility and decoupling between the PaymentService and the notification mechanism.
    private readonly INotificationService _notificationService;
    public PaymentService(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }
    public void Process()
    {
        Console.WriteLine("Payment process completed.");
        _notificationService.Send();
    }
}
// Here, we define an interface INotificationService that has a Send method. This allows us to have different implementations of the notification service (like EmailService, SMSService, etc.) without changing the PaymentService code. The PaymentService only depends on the abstraction (INotificationService) and not on any concrete implementation, which is a key principle of DIP.
public interface INotificationService
{
    public void Send();
}
// Here, EmailService can come from other software, and we don't need to change our PaymentService code to use it. We just need to register it in our service collection, and the dependency injection framework will take care of the rest.
public class EmailService : INotificationService
{
    public void Send()
    {
        Console.WriteLine("Email sent.");
    }
}

