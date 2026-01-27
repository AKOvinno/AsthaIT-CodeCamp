BankAccount account = new BankAccount(1000m); // Create account with initial balance
Console.WriteLine($"Current Balance: {account.Balance}"); // Output current balance
account.Withdraw(200m); // Withdraw some money
Console.WriteLine($"Balance after withdrawal: {account.Balance}"); // Output balance after withdrawal
class BankAccount
{
    private decimal _balance; // Private field: cannot be accessed directly from outside
    public BankAccount(decimal initialBalance) // Constructor to initialize account
    {
        if(initialBalance < 0)
            throw new ArgumentException("Initial balance cannot be negative."); // Protect invariant
        _balance = initialBalance;
    }
    public void Withdraw(decimal amount)
    {
        if(amount <= 0)
            throw new ArgumentException("Withdrawal amount must be positive."); // Validate input
        if(amount > _balance)
            throw new InvalidOperationException("Insufficient funds."); // Protect state
        _balance -= amount; // Update internal state safely
    }
    public decimal Balance => _balance; // Public getter provides controlled read access
}
