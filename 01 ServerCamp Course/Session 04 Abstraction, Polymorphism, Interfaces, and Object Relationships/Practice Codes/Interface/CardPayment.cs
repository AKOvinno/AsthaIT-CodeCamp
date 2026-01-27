class CardPayment : IPayment, INotification
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"Paid {amount} using card");
    }
    public void PrintReceipt()
    {
        Console.WriteLine("Receipt printed");
    }
    public void SendNotification(string message)
    {
        Console.WriteLine($"Card Payment Notification: {message}");
    }
}