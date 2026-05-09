// ================================================================
//  03 — Generic Methods
//
//  Not just classes — individual methods can be generic too.
//  The type parameter T is declared on the method itself.
// ================================================================

using GenericsLab.Models;

namespace GenericsLab.Concepts;

public static class Concept03_GenericMethods
{
    public static void Run()
    {
        Console.WriteLine("=== 03: Generic Methods ===\n");

        // ── A generic method: Print<T> ────────────────────────────
        // You can call it with any type
        Print(new Product { Id = 1, Name = "Keyboard", Price = 89.99m });
        Print(new Order   { Id = 1, Item = "Monitor",  Quantity = 2   });
        Print("Hello from a string!");
        Print(42);

        Console.WriteLine();

        // ── A generic method that returns T ───────────────────────
        Product p = CreateDefault<Product>();
        Order   o = CreateDefault<Order>();

        Console.WriteLine($"Default Product: {p}");
        Console.WriteLine($"Default Order:   {o}");

        Console.WriteLine();

        // ── Where generics are inferred ───────────────────────────
        // C# can often figure out T from the argument you pass
        // You don't always need to write Print<Product>(...)
        // C# sees you're passing a Product and infers T = Product
        Print(new Product { Name = "Inferred!" });  // T inferred as Product

        Console.WriteLine();

        // ── How MiniOrm uses generic methods ──────────────────────
        // DbContext has: protected DbSet<T> Set<T>() where T : new()
        // This is a generic method that creates a DbSet<T>
        // When called as Set<Product>() → returns DbSet<Product>
        Console.WriteLine("-- MiniOrm connection --");
        Console.WriteLine("DbContext.Set<Product>() returns DbSet<Product>");
        Console.WriteLine("DbContext.Set<Order>()   returns DbSet<Order>");
        Console.WriteLine("Same method. Different T each time.");
        Console.WriteLine();
    }

    // ── Generic method definition ─────────────────────────────────
    // <T> goes between the method name and the parameters
    public static void Print<T>(T item)
    {
        Console.WriteLine($"[{typeof(T).Name}] {item}");
    }

    // Generic method that creates and returns a T
    // where T : new() means T must have a parameterless constructor
    public static T CreateDefault<T>() where T : new()
    {
        return new T();   // creates a new instance of whatever T is
    }
}

// ── Key takeaway ─────────────────────────────────────────────────
// Generic methods declare <T> on the method signature.
// Each call can use a different T.
// C# often infers T from the argument — you don't always
// need to write the type explicitly.
