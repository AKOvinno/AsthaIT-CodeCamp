// We will define a common interface that will support both Bkash and Nagad APIs
interface IPaymentGateway
{
    void Pay(decimal amount);
}
// Here it's breaking the LSP because Nagad API expects an integer amount, but we are using decimal in the interface. To fix this, we can use method overloading or create a separate interface for Nagad. For simplicity, let's use method overloading in the adapter classes.
class BkashPaymentAdapter : IPaymentGateway
{
    private BkashAPI _bkashAPI = new BkashAPI(); // provided by Bkash
    public void Pay(decimal amount)
    {
        _bkashAPI.Pay(amount);
    }
}
class NagadPaymentAdapter : IPaymentGateway
{
    private NagadAPI _nagadAPI = new NagadAPI(); // provided by Nagad
    public void Pay(decimal amount)
    {
        // Here we can add additional steps specific to Nagad before calling the API and it's called decorator pattern as well because we are adding additional responsibilities to the Pay method without changing its interface.
        // Step 1: logging to database
        Console.WriteLine($"Logging payment of {amount} BDT to database for Nagad.");
        // Step 2: Notification to user
        Console.WriteLine($"Notifying user about payment of {amount} BDT through Nagad.");
        // Step 3: Call Nagad API
        _nagadAPI.Pay((int)amount); // Nagad API expects an integer amount
    }
}
// ========== Owner of this code is Bkash Company ==========
class BkashAPI
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"Processing payment of {amount} BDT through Bkash.");

    }
}
// ========== Owner of this code is Nagad Company ==========
class NagadAPI
{
    public void Pay(int amount)
    {
        Console.WriteLine($"Sending {amount} BDT through Nagad.");
    }
}