IPayment paymentMethod = PaymentFactory.GetPaymentMethod("bkash");
paymentMethod.Pay(1000);

IReceiptGenerator receiptGenerator = ReceiptFactory.GetReceiptGenerator("email");
receiptGenerator.GenerateReceipt();
class PaymentFactory
{
    public static IPayment GetPaymentMethod(string method)
    {
        return method.ToLower() switch
        {
            "bkash" => new BkashPayment(),
            "rocket" => new RocketPayment(),
            _ => throw new ArgumentException("Invalid payment method")
        };
    }
}
interface IPayment
{
    void Pay(decimal amount);
}
class BkashPayment : IPayment
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"💰 Paid {amount} Taka via Bkash");
    }
}
class RocketPayment : IPayment
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"💰 Paid {amount} Taka via Rocket");
    }
}
class ReceiptFactory
{
    public static IReceiptGenerator GetReceiptGenerator(string method)
    {
        return method.ToLower() switch
        {
            "paper" => new PaperReceiptGenerator(),
            "email" => new EmailReceiptGenerator(),
            _ => throw new ArgumentException("Invalid receipt generator type")
        };
    }
}
interface IReceiptGenerator
{
    void GenerateReceipt();
}
class PaperReceiptGenerator : IReceiptGenerator
{
    public void GenerateReceipt()
    {
        Console.WriteLine("🧾 Generated paper receipt");
    }
}
class EmailReceiptGenerator : IReceiptGenerator
{
    public void GenerateReceipt()
    {
        Console.WriteLine("✉️ Generated email receipt");
    }
}