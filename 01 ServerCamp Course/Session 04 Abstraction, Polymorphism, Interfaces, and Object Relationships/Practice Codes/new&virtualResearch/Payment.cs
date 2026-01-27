class Payment
{
    // Abstract method: must be implemented by derived class
    public virtual void Pay(decimal amount)
    {
        Console.WriteLine($"Paid {amount} using card from base class");
    }
    
    // Concrete method: optional, inherited as-is
    public void PrintReceipt()
    {
        Console.WriteLine("Receipt printed");
    }
    public void ApplyDiscount()
    {
        Console.WriteLine("Default discount applied");
    }
}