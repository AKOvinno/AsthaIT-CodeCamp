public class CreditCardProcessor : IPaymentProcessor
{
    public bool Pay(string paymentInfo, double amount)
    {
        // paymentInfo is the credit card number
        Console.WriteLine($"Credit Card: Processing ${amount} payment with card {paymentInfo}");
        Console.WriteLine("Credit Card: Payment successful");
        return true;
    }
    public string GetPaymentMethod() => "Credit Card";
}
