// Derived class: PayPalPayment
class PayPalPayment : Payment
{
    // Must override abstract method
    public override void Pay(decimal amount)
    {
        Console.WriteLine($"Paid {amount} via PayPal");
    }
    public new void ApplyDiscount()
    {
        base.ApplyDiscount();
        Console.WriteLine("Applied new PayPal discount");
    }
}