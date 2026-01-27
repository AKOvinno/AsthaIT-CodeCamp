// Derived class: CardPayment
class CardPayment : Payment
{
    //Must override abstract method
    public override void Pay(decimal amount)
    {
        Console.WriteLine($"Paid {amount} using card");
    }
    // Optional: override virtual method
    public override void ApplyDiscount()
    {
        Console.WriteLine("10% Card discount applied");
    }
}