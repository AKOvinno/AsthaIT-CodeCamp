class MobilePayment : IPayment, INotification
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"Paid {amount} using mobile payment method");
    }
    public void PrintReceipt()
    {
        Console.WriteLine("Mobile Payment Receipt printed");
    }
    public void SendNotification(string message)
    {
        Console.WriteLine($"Mobile Payment Notification: {message}");
    }
}