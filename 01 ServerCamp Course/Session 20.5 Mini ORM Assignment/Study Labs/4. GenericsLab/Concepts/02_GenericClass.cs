// ================================================================
//  02 — Generic Classes
//
//  How to define a class with a type parameter T.
//  T is just a placeholder — it gets replaced with the
//  real type when you use the class.
// ================================================================

using GenericsLab.Models;

namespace GenericsLab.Concepts;

public static class Concept02_GenericClass
{
    public static void Run()
    {
        Console.WriteLine("=== 02: Generic Classes ===\n");

        // ── Using a generic class ─────────────────────────────────

        // T becomes Product here
        var productRepo = new SimpleRepository<Product>();
        productRepo.Add(new Product { Id = 1, Name = "Keyboard", Price = 89.99m });
        productRepo.Add(new Product { Id = 2, Name = "Mouse",    Price = 29.99m });

        Console.WriteLine("-- Products --");
        foreach (var p in productRepo.GetAll())
            Console.WriteLine($"  {p}");

        var found = productRepo.FindById(1);
        Console.WriteLine($"Found by Id=1: {found}");

        Console.WriteLine();

        // T becomes Order here
        var orderRepo = new SimpleRepository<Order>();
        orderRepo.Add(new Order { Id = 1, Item = "Monitor", Quantity = 2 });

        Console.WriteLine("-- Orders --");
        foreach (var o in orderRepo.GetAll())
            Console.WriteLine($"  {o}");

        Console.WriteLine();

        // ── Key point: T is decided at USAGE time ─────────────────
        // When you write SimpleRepository<Product>
        //   → every T inside the class becomes Product
        // When you write SimpleRepository<Order>
        //   → every T inside the class becomes Order

        Console.WriteLine("-- Type check --");
        Console.WriteLine($"productRepo type: {productRepo.GetType().Name}");  // SimpleRepository`1
        Console.WriteLine($"T inside productRepo: {typeof(Product).Name}");    // Product
        Console.WriteLine();
    }
}

// ── A simple generic repository ───────────────────────────────────
// This is a stripped-down version of MiniOrm's DbSet<T>.
// Instead of talking to PostgreSQL, it uses an in-memory list.
// The STRUCTURE is identical to DbSet<T>.

public class SimpleRepository<T> where T : class
{
    // _items is a List<T> — a list of whatever T is
    private readonly List<T> _items = new();

    // Add accepts a T — if T is Product, this accepts a Product
    public void Add(T item)
    {
        _items.Add(item);
    }

    // GetAll returns IEnumerable<T> — a sequence of T
    public IEnumerable<T> GetAll()
    {
        return _items;
    }

    // FindById uses reflection to find the Id property
    // This is exactly what DbSet<T>.FindByIdAsync() does
    public T? FindById(int id)
    {
        var idProp = typeof(T).GetProperty("Id");
        return _items.FirstOrDefault(item =>
        {
            var value = idProp?.GetValue(item);
            return value is int intVal && intVal == id;
        });
    }
}

// ── Key takeaway ─────────────────────────────────────────────────
// A generic class is defined with <T> after the class name.
// T is just a name — you could call it anything (X, TEntity etc.)
// but T is the convention.
// The real type is substituted in when you USE the class:
//   SimpleRepository<Product> → T = Product everywhere inside
