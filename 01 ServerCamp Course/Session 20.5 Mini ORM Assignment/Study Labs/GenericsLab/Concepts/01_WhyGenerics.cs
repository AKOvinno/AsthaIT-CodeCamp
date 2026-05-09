// ================================================================
//  01 — Why Do We Need Generics?
//
//  The problem: without generics, you have to write the same
//  code over and over for every type.
//
//  The solution: generics let you write code ONCE and reuse it
//  for ANY type.
// ================================================================

using GenericsLab.Models;

namespace GenericsLab.Concepts;

public static class Concept01_WhyGenerics
{
    public static void Run()
    {
        Console.WriteLine("=== 01: Why Do We Need Generics? ===\n");

        // ── The problem WITHOUT generics ──────────────────────────
        // Imagine you need a "box" that holds one item.
        // Without generics, you need a separate class for each type:

        var productBox = new ProductBox();
        productBox.Item = new Product { Id = 1, Name = "Keyboard" };
        Console.WriteLine($"ProductBox holds: {productBox.Item.Name}");

        var orderBox = new OrderBox();
        orderBox.Item = new Order { Id = 1, Item = "Monitor" };
        Console.WriteLine($"OrderBox holds: {orderBox.Item.Item}");

        // Problem: ProductBox and OrderBox are identical code
        // duplicated for each type. If you have 10 types, you
        // need 10 identical Box classes. That is bad.

        Console.WriteLine();

        // ── The solution WITH generics ────────────────────────────
        // One Box<T> class works for ANY type:

        var gProductBox = new Box<Product>();
        gProductBox.Item = new Product { Id = 1, Name = "Keyboard" };
        Console.WriteLine($"Box<Product> holds: {gProductBox.Item.Name}");

        var gOrderBox = new Box<Order>();
        gOrderBox.Item = new Order { Id = 1, Item = "Monitor" };
        Console.WriteLine($"Box<Order> holds: {gOrderBox.Item.Item}");

        // Same class, different types. Write once, use everywhere.

        Console.WriteLine();

        // ── How this applies to MiniOrm ───────────────────────────
        // DbSet<T> is exactly this idea.
        // Without generics you would need:
        //   ProductDbSet  → Insert(Product), FindById, GetAll...
        //   OrderDbSet    → Insert(Order),   FindById, GetAll...
        //
        // With generics you write DbSet<T> ONCE and it works
        // for Product, Order, and any future entity.

        Console.WriteLine("-- MiniOrm connection --");
        Console.WriteLine("DbSet<Product> handles: Insert, FindById, GetAll, Update, Delete for Product");
        Console.WriteLine("DbSet<Order>   handles: Insert, FindById, GetAll, Update, Delete for Order");
        Console.WriteLine("Same code. Different T.");
        Console.WriteLine();
    }
}

// ── Without generics — one class per type (BAD) ───────────────────
public class ProductBox
{
    public Product Item { get; set; } = null!;
}

public class OrderBox
{
    public Order Item { get; set; } = null!;
}

// ── With generics — one class for ALL types (GOOD) ────────────────
public class Box<T>
{
    // T is a placeholder. When you write Box<Product>, T becomes Product.
    // When you write Box<Order>, T becomes Order.
    public T Item { get; set; } = default!;
}
