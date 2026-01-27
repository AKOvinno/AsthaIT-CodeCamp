Console.WriteLine("Card Payment:");
Payment card = new CardPayment();
card.Pay(500);           // doesn't have its own Pay implementation, uses base class
card.PrintReceipt();     // Inherited concrete method
card.ApplyDiscount();    
Console.WriteLine("\nPayPal Payment:");
Payment paypal = new PayPalPayment();
paypal.Pay(300);         // Uses PayPalPayment implementation
paypal.PrintReceipt();   // Inherited concrete method
paypal.ApplyDiscount();
Console.WriteLine();

// Demonstrating method hiding with 'new' keyword
// Directly calling ApplyDiscount on CardPayment instance
// versus through Payment reference
// to show the difference
// between the two implementations
// of ApplyDiscount.
// The 'new' keyword hides the base class method.
// When called on the derived class instance, the derived class method is invoked.
// When called on the base class reference, the base class method is invoked.
// In this case, the base class method is overridden by the derived class method.
// This demonstrates method hiding in C#.
// Output will show the difference.
Console.WriteLine("Direct CardPayment Call:");
CardPayment cardPayment = new CardPayment();
cardPayment.ApplyDiscount(); // Calls CardPayment's ApplyDiscounts
Console.WriteLine("Payment Class Call:");
Payment payment = new CardPayment();
payment.ApplyDiscount(); // Calls Payment's ApplyDiscount


