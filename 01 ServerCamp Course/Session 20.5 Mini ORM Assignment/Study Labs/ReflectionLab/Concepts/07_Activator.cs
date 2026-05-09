// ================================================================
//  07 — Activator.CreateInstance()
//  Creates an object without using "new" directly.
//  Used when you don't know the exact type at compile time.
// ================================================================

using System.Reflection;

namespace ReflectionLab.Concepts;

public static class Concept07_Activator
{
    public static void Run()
    {
        Console.WriteLine("=== 07: Activator.CreateInstance() ===\n");

        // ── Simple — no constructor arguments ─────────────────────
        // Same as: var p = new Product();
        Type productType = typeof(Product);
        object obj       = Activator.CreateInstance(productType)!;
        Product product  = (Product)obj;

        Console.WriteLine($"Created Product. Name = '{product.Name}'");  // (empty string)

        // ── With a generic type built at runtime ──────────────────
        // Imagine we don't know it's DbSet<Product> at compile time —
        // we figured it out by scanning AppDbContext's properties.
        Type openDbSet    = typeof(DbSet<>);
        Type entityType   = typeof(Product);

        // MakeGenericType fills in T — creates DbSet<Product> type
        Type closedDbSet  = openDbSet.MakeGenericType(entityType);
        Console.WriteLine($"\nBuilt type: {closedDbSet.Name}");           // DbSet`1

        // Now create an instance — same as: new DbSet<Product>()
        object dbSetObj   = Activator.CreateInstance(closedDbSet)!;
        Console.WriteLine($"Created: {dbSetObj.GetType().GetGenericArguments()[0].Name} DbSet"); // Product DbSet

        // ── How DbContext.InitializeSets() uses this ──────────────
        // It scans AppDbContext, finds DbSet<Product> and DbSet<Order>,
        // creates instances of each, and assigns them to the properties.
        Console.WriteLine("\n-- Simulating InitializeSets() --");

        Type contextType  = typeof(AppDbContext);
        Type dbSetType    = typeof(DbSet<>);
        var  fakeContext  = new AppDbContext();

        foreach (var prop in contextType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!prop.PropertyType.IsGenericType) continue;
            if (prop.PropertyType.GetGenericTypeDefinition() != dbSetType) continue;

            // Create new DbSet<T>() where T is whatever we found
            object instance = Activator.CreateInstance(prop.PropertyType)!;

            // Assign it to the property on the context
            prop.SetValue(fakeContext, instance);

            Type entity = prop.PropertyType.GetGenericArguments()[0];
            Console.WriteLine($"  Set {prop.Name} → new DbSet<{entity.Name}>() ✓");
        }

        Console.WriteLine($"\n  fakeContext.Products is null? {fakeContext.Products == null}");  // False
        Console.WriteLine($"  fakeContext.Orders   is null? {fakeContext.Orders   == null}");  // False
        Console.WriteLine();
    }
}

// ── Fake AppDbContext for this demo (no Npgsql needed) ────────────
// public class AppDbContext
// {
//     public DbSet<Product> Products { get; set; } = null!;
//     public DbSet<Order>   Orders   { get; set; } = null!;
// }

// ── Key takeaway ─────────────────────────────────────────────────
// Activator.CreateInstance(type)       → new T() with no args
// Activator.CreateInstance(type, args) → new T(arg1, arg2...)
// MakeGenericType(entityType)          → fills T into DbSet<>
//                                        to get DbSet<Product>
//
// Without Activator, DbContext would need to manually write:
//   Products = new DbSet<Product>(connStr);
//   Orders   = new DbSet<Order>(connStr);
// With Activator it happens automatically for ANY DbSet<T>.
