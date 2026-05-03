public interface IPaymentProcessor
{
    bool Pay(string paymentInfo, double amount);
    string GetPaymentMethod();
}
