Console.WriteLine("\nCard Payment:");
CardPayment payment = new CardPayment();
payment.Pay(500);
payment.PrintReceipt();
payment.SendNotification("Payment completed");

Console.WriteLine("\nAdvanced Payment:");
AdvancedPayment advancedPayment = new ();
advancedPayment.Pay(1000);
advancedPayment.PrintReceipt();
advancedPayment.SendNotification("Payment completed");


Console.WriteLine("\nMobile Payment:");
MobilePayment mobilePayment = new ();
mobilePayment.Pay(200);
mobilePayment.PrintReceipt();
mobilePayment.SendNotification("Payment completed");

// Using interfaces to demonstrate polymorphism
// INotification interface reference pointing to MobilePayment object to send a notification
// without needing to know the specific payment type
// The SendNotification method is defined in the INotification interface
// and implemented by the MobilePayment class
// This allows for flexibility and decouples the notification logic from the payment logic
// making the code more maintainable and extensible.
INotification notification = new MobilePayment();
notification.SendNotification("This is a notification for mobile payment.");
