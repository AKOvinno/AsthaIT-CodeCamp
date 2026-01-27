// Derived class: CardPayment
class CardPayment : Payment
{
    // Must override abstract method
    // public override void Pay(decimal amount)
    // {
    //     Console.WriteLine($"Paid {amount} using card");
    // }
    
    // Optional: override virtual method
    public new void ApplyDiscount()
    {
        Console.WriteLine("10% new Card discount applied");
    }
}