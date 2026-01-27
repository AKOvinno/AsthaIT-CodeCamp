Console.WriteLine("Card Payment:");
Payment card = new CardPayment();
card.Pay(500);           // Uses CardPayment implementation
card.PrintReceipt();     // Inherited concrete method
card.ApplyDiscount();    
Console.WriteLine("\nPayPal Payment:");
Payment paypal = new PayPalPayment();
paypal.Pay(300);         // Uses PayPalPayment implementation
paypal.PrintReceipt();   // Inherited concrete method
paypal.ApplyDiscount();
Console.WriteLine();
