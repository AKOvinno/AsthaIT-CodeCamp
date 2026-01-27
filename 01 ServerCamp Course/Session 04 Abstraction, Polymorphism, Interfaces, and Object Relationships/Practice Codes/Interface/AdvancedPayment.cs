class AdvancedPayment: IPayment, INotification
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"Paid {amount} using advanced payment method");
    }
    public void PrintReceipt()
    {
        Console.WriteLine("Advanced Payment Receipt printed");
    }
    public void SendNotification(string message)
    {
        Console.WriteLine($"Advanced Payment Notification: {message}");
    }
}