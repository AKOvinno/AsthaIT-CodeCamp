public class BkashPaymentAdapter : IPaymentProcessor
{
    private readonly BkashPaymentGateway _gateway;

    public BkashPaymentAdapter()
    {
        _gateway = new BkashPaymentGateway();
    }

    public bool Pay(string paymentInfo, double amount)
    {
        // paymentInfo is the phone number for bKash
        string transactionId = _gateway.SendMoney(paymentInfo, amount);
        return _gateway.CheckStatus(transactionId);
    }

    public string GetPaymentMethod() => "bKash";
}
