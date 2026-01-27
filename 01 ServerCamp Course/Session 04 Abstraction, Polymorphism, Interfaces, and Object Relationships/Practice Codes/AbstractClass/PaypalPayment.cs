// Derived class: PayPalPayment
class PayPalPayment : Payment
{
    // Must override abstract method
    public override void Pay(decimal amount)
    {
        Console.WriteLine($"Paid {amount} via PayPal");
    }
    public override void ApplyDiscount()
    {
        Console.WriteLine("15% PayPal discount applied");
    }
}