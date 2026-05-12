// ================================================================
//  04 — Generic Constraints (where T : ...)
//
//  By default T can be ANYTHING — int, string, Product, etc.
//  Constraints let you restrict what T can be.
//  This unlocks features on T that you couldn't use otherwise.
// ================================================================

using GenericsLab.Models;

namespace GenericsLab.Concepts;

public static class Concept04_Constraints
{
    public static void Run()
    {
        Console.WriteLine("=== 04: Generic Constraints ===\n");

        // ── where T : new() ───────────────────────────────────────
        // T must have a parameterless constructor.
        // This lets you write: new T() inside the class/method.
        // MiniOrm uses this in DbSet<T> so it can create blank
        // entity objects when mapping database rows.

        Console.WriteLine("-- where T : new() --");
        var p = Factory.Create<Product>();
        var o = Factory.Create<Order>();
        Console.WriteLine($"Created: {p}");
        Console.WriteLine($"Created: {o}");

        Console.WriteLine();

        // ── where T : class ───────────────────────────────────────
        // T must be a reference type (a class, not int/bool/decimal).
        // This lets you return null for T.
        // SimpleRepository<T> uses this so FindById can return null.

        Console.WriteLine("-- where T : class --");
        var repo = new NullableRepository<Product>();
        repo.Add(new Product { Id = 1, Name = "Keyboard" });
        var found    = repo.Find(1);
        var notFound = repo.Find(99);
        Console.WriteLine($"Found Id=1:  {found}");
        Console.WriteLine($"Found Id=99: {notFound ?? (object)"null"}");  // null

        Console.WriteLine();

        // ── Combining constraints ─────────────────────────────────
        // You can combine them: where T : class, new()
        // MiniOrm's DbSet<T> does exactly this:
        //   public sealed class DbSet<T> where T : new()
        //
        // new()  → so DbSet can call new T() when mapping rows
        // (class constraint is often skipped when new() is enough)

        Console.WriteLine("-- where T : class, new() combined --");
        var combo = new ComboExample<Product>();
        combo.Demonstrate();

        Console.WriteLine();
    }
}

// ── where T : new() ──────────────────────────────────────────────
public static class Factory
{
    // Without "where T : new()" the compiler would reject "new T()"
    // because T might be something without a constructor (like int).
    public static T Create<T>() where T : new()
    {
        return new T();   // only allowed because of the constraint
    }
}

// ── where T : class ──────────────────────────────────────────────
public class NullableRepository<T> where T : class
{
    private readonly List<T> _items = new();

    public void Add(T item) => _items.Add(item);

    // Without "where T : class", returning null here would be
    // a compiler error because T might be a value type like int.
    public T? Find(int id)
    {
        var idProp = typeof(T).GetProperty("Id");
        return _items.FirstOrDefault(item =>
            (int?)idProp?.GetValue(item) == id);
    }
}

// ── Combined constraints ──────────────────────────────────────────
public class ComboExample<T> where T : class, new()
{
    public void Demonstrate()
    {
        T instance = new T();          // allowed because of new()
        T? nullable = null;            // allowed because of class
        Console.WriteLine($"  Created {typeof(T).Name}, nullable assigned: {nullable == null}");
    }
}

// ── Key takeaway ─────────────────────────────────────────────────
// where T : new()    → you can call new T() inside the class
// where T : class    → T is a reference type, so T? is allowed
// where T : SomeBase → T must inherit from SomeBase
// These are the three you'll see most often.
// MiniOrm uses: where T : new()  in DbSet<T>
