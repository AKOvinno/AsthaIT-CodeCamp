// Example interface for payment
// This interface defines what any payment class should do
interface IPayment
{
    void Pay(decimal amount); // Must be implemented
    void PrintReceipt(); // Must be implemented
}