// ================================================================
//  07 — Building a Mini DbSet<T> from Scratch
//
//  This is the big payoff concept. We build a simplified
//  version of MiniOrm's DbSet<T> using everything learned
//  in concepts 01-06 — but backed by an in-memory list
//  instead of PostgreSQL so you can run it without a database.
//
//  After understanding this file, MiniOrm's real DbSet<T>
//  will make complete sense.
// ================================================================

using System.Reflection;
using GenericsLab.Models;

namespace GenericsLab.Concepts;

public static class Concept07_MiniOrmDbSet
{
    public static void Run()
    {
        Console.WriteLine("=== 07: Building a Mini DbSet<T> ===\n");

        // ── Use DbSet<T> just like in MiniOrm ────────────────────
        var products = new FakeDbSet<Product>();

        // Insert
        int id1 = products.Insert(new Product { Name = "Keyboard", Price = 89.99m });
        int id2 = products.Insert(new Product { Name = "Mouse",    Price = 29.99m });
        Console.WriteLine();

        // FindById
        var found = products.FindById(id1);
        Console.WriteLine($"FindById({id1}): {found}");
        Console.WriteLine();

        // GetAll
        Console.WriteLine("GetAll:");
        foreach (var p in products.GetAll())
            Console.WriteLine($"  {p}");
        Console.WriteLine();

        // Update
        found!.Price = 79.99m;
        products.Update(found);
        Console.WriteLine();

        // Delete
        products.Delete(id2);
        Console.WriteLine();

        // Final state
        Console.WriteLine("Final GetAll:");
        foreach (var p in products.GetAll())
            Console.WriteLine($"  {p}");

        Console.WriteLine();

        // ── Same DbSet for Order — no new code needed ─────────────
        Console.WriteLine("-- Now with Order (same DbSet<T> code) --");
        var orders = new FakeDbSet<Order>();
        orders.Insert(new Order { Item = "Monitor", Quantity = 1 });
        orders.Insert(new Order { Item = "Desk",    Quantity = 2 });
        foreach (var o in orders.GetAll())
            Console.WriteLine($"  {o}");

        Console.WriteLine();
    }
}

// ================================================================
//  FakeDbSet<T> — in-memory version of MiniOrm's DbSet<T>
//
//  Real DbSet<T> uses NpgsqlCommand to run SQL.
//  FakeDbSet<T> uses a List<T> instead.
//  The GENERIC STRUCTURE is identical.
// ================================================================

public class FakeDbSet<T> where T : new()
{
    // ── Concept 01 & 02: T makes this work for any entity ────────
    private readonly List<T> _store = new();
    private int _nextId = 1;

    // ── Concept 06: typeof(T) to get the entity's properties ─────
    private readonly Type _type = typeof(T);

    // ── Concept 04: where T : new() lets us call new T() ─────────

    // ── Insert ────────────────────────────────────────────────────
    public int Insert(T entity)
    {
        // Set the Id using reflection (Concept 03 — SetValue)
        var idProp = _type.GetProperty("Id")!;
        idProp.SetValue(entity, _nextId);

        _store.Add(entity);

        // Print all property values using reflection (Concept 03 — GetValue)
        var values = string.Join(", ", _type.GetProperties()
            .Select(p => $"{p.Name}={p.GetValue(entity) ?? "NULL"}"));
        Console.WriteLine($"Inserted {_type.Name}: [{values}]");

        return _nextId++;
    }

    // ── FindById ──────────────────────────────────────────────────
    public T? FindById(int id)
    {
        var idProp = _type.GetProperty("Id")!;

        // Loop through stored items and compare Id values
        // GetValue reads the Id property via reflection
        return _store.FirstOrDefault(item =>
            (int?)idProp.GetValue(item) == id);
    }

    // ── GetAll ────────────────────────────────────────────────────
    public IEnumerable<T> GetAll() => _store;

    // ── Update ────────────────────────────────────────────────────
    public void Update(T entity)
    {
        var idProp  = _type.GetProperty("Id")!;
        var entityId = (int)idProp.GetValue(entity)!;

        // Find the existing item with this Id
        var existing = FindById(entityId);
        if (existing == null) return;

        // Copy every property from entity → existing using reflection
        foreach (var prop in _type.GetProperties())
        {
            if (prop.Name == "Id") continue;               // skip PK
            var value = prop.GetValue(entity);
            prop.SetValue(existing, value);                // SetValue!
        }

        var values = string.Join(", ", _type.GetProperties()
            .Select(p => $"{p.Name}={p.GetValue(existing) ?? "NULL"}"));
        Console.WriteLine($"Updated {_type.Name}: [{values}]");
    }

    // ── Delete ────────────────────────────────────────────────────
    public void Delete(int id)
    {
        var item = FindById(id);
        if (item == null) return;
        _store.Remove(item);
        Console.WriteLine($"Deleted {_type.Name} Id={id} ✓");
    }
}

// ── Key takeaway ─────────────────────────────────────────────────
// Every generic concept comes together here:
//
//  01 WhyGenerics     → one FakeDbSet<T> works for Product AND Order
//  02 GenericClass    → class FakeDbSet<T> definition
//  03 GenericMethods  → Insert(T entity), FindById returns T?
//  04 Constraints     → where T : new() so we can call new T()
//  05 Interfaces      → FakeDbSet could implement IRepository<T>
//  06 default/typeof  → typeof(T) to get properties via reflection
//                       default returned when FindById finds nothing
//
// The ONLY difference between FakeDbSet<T> and MiniOrm's DbSet<T>:
//   FakeDbSet  → stores in List<T>
//   Real DbSet → runs parameterised SQL via NpgsqlCommand
