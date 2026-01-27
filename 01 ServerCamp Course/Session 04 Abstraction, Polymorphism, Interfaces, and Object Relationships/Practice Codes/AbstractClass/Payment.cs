abstract class Payment
{
    // Abstract method: must be implemented by derived class
    public abstract void Pay(decimal amount);
    // Concrete method: optional, inherited as-is
    public void PrintReceipt()
    {
        Console.WriteLine("Receipt printed");
    }
    public virtual void ApplyDiscount()
    {
        Console.WriteLine("Default discount applied");
    }
}