// ================================================================
//  Models used across all concept files
//  These are simplified versions of MiniOrm's Product and Order
// ================================================================

namespace GenericsLab.Models;

public class Product
{
    public int     Id    { get; set; }
    public string  Name  { get; set; } = string.Empty;
    public decimal Price { get; set; }

    public override string ToString() => $"Product {{ Id={Id}, Name={Name}, Price={Price} }}";
}

public class Order
{
    public int    Id       { get; set; }
    public int    Quantity { get; set; }
    public string Item     { get; set; } = string.Empty;

    public override string ToString() => $"Order {{ Id={Id}, Item={Item}, Quantity={Quantity} }}";
}
