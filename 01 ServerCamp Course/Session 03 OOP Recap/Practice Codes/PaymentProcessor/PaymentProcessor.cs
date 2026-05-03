public abstract class PaymentProcessor
{
    public string TransactionId { get; protected set; }
    public decimal Amount { get; protected set; }
    protected PaymentProcessor(decimal amount)
    {
        Amount = amount;
        TransactionId = Guid.NewGuid().ToString();
    }
    public abstract bool ProcessPayment();
    public virtual void LogTransaction(string details)
    {
        Console.WriteLine($"[{DateTime.UtcNow}] Transaction {TransactionId}: {details}");
    }
}
public class CreditCardPaymentProcessor : PaymentProcessor
{
    private readonly string _cardNumber;
    public CreditCardPaymentProcessor(decimal amount, string cardNumber) : base(amount)
    {
        _cardNumber = cardNumber;
    }
    public override bool ProcessPayment()
    {
        LogTransaction("Processing credit card payment.");
        return true; // Simulate success
    }
}
public class PayPalPaymentProcessor : PaymentProcessor
{
    private readonly string _email;
    public PayPalPaymentProcessor(decimal amount, string email) : base(amount)
    {
        _email = email;
    }
    public override bool ProcessPayment()
    {
        LogTransaction("Processing PayPal payment.");
        return true; // Simulate success
    }
}
public class BkashPaymentProcessor : PaymentProcessor
{
    private readonly string _phoneNumber;
    public BkashPaymentProcessor(decimal amount, string phoneNumber) : base(amount)
    {
        _phoneNumber = phoneNumber;
    }
    public override bool ProcessPayment()
    {
        LogTransaction("Processing Bkash payment.");
        return true; // Simulate success
    }
}