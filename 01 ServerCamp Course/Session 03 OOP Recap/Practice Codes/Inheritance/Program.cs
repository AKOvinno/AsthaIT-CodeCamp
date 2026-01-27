Product product = new Product(1, "Laptop", 999.99m);
product.DisplayInfo();
// Base class: BaseEntity
public class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public BaseEntity(int id)
    {
        Id = id;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }
    public void UpdateTimestamp()
    {
        UpdatedAt = DateTime.Now;
        Console.WriteLine($"Entity {Id} updated at {UpdatedAt}");
    }
}
// Subclass: Product
public class Product : BaseEntity
{
    public string Name { get; set; }
    public decimal Price { get; set; }

    public Product(int id, string name, decimal price) : base(id)
    {
        Name = name;
        Price = price;
    }
    public void DisplayInfo()
    {
        Console.WriteLine($"Product: {Name}, Price: {Price:C}");
    }
}
// Subclass: Customer
public class Customer : BaseEntity
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public Customer(int id, string fullName, string email) : base(id)
    {
        FullName = fullName;
        Email = email;
    }
    public void DisplayInfo()
    {
        Console.WriteLine($"Customer: {FullName}, Email: {Email}");
    }
}

