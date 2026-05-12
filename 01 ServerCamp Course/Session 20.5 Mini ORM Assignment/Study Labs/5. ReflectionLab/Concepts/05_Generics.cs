// ================================================================
//  05 — Generics and Reflection
//  Generic types like DbSet<T> need special handling.
//  There are two kinds: open generics and closed generics.
// ================================================================

using System.Reflection;

namespace ReflectionLab.Concepts;

// A simple generic class to experiment with
public class DbSet<T>
{
    public string EntityTypeName => typeof(T).Name;
}

// A fake AppDbContext to scan
// public class AppDbContext
// {
//     public DbSet<Product> Products { get; set; } = null!;
//     public DbSet<Order>   Orders   { get; set; } = null!;
//     public string         Name     { get; set; } = "MyApp";   // NOT a DbSet
// }

public static class Concept05_Generics
{
    public static void Run()
    {
        Console.WriteLine("=== 05: Generics and Reflection ===\n");

        // ── Open vs closed generic types ──────────────────────────
        Type open   = typeof(DbSet<>);           // T is NOT filled in — "open generic"
        Type closed = typeof(DbSet<Product>);    // T = Product      — "closed generic"

        Console.WriteLine($"open.IsGenericType   : {open.IsGenericType}");    // True
        Console.WriteLine($"closed.IsGenericType : {closed.IsGenericType}");  // True

        // GetGenericTypeDefinition() strips T away from a closed generic
        // DbSet<Product> → DbSet<>
        Console.WriteLine($"closed.GetGenericTypeDefinition() == open : " +
            $"{closed.GetGenericTypeDefinition() == open}");  // True

        // GetGenericArguments() tells you what T actually is
        Type entityType = closed.GetGenericArguments()[0];
        Console.WriteLine($"T inside DbSet<Product> : {entityType.Name}");    // Product

        // ── How DbContext.InitializeSets() uses this ──────────────
        // It scans AppDbContext properties and finds all DbSet<T> ones.
        Console.WriteLine("\n-- Scanning AppDbContext for DbSet<T> properties --");

        Type dbSetOpenType = typeof(DbSet<>);

        foreach (var prop in typeof(AppDbContext).GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            // Step 1: skip if not generic at all (e.g. string Name)
            if (!prop.PropertyType.IsGenericType)
            {
                Console.WriteLine($"  {prop.Name,-12} → NOT generic, skipped");
                continue;
            }

            // Step 2: skip if generic but not DbSet<>
            if (prop.PropertyType.GetGenericTypeDefinition() != dbSetOpenType)
            {
                Console.WriteLine($"  {prop.Name,-12} → generic but not DbSet<>, skipped");
                continue;
            }

            // Step 3: it IS a DbSet<T> — find out which T
            Type entity = prop.PropertyType.GetGenericArguments()[0];
            Console.WriteLine($"  {prop.Name,-12} → DbSet<{entity.Name}> ✓");
        }

        Console.WriteLine();
    }
}

// ── Key takeaway ─────────────────────────────────────────────────
// typeof(DbSet<>)           → the "template" open generic
// typeof(DbSet<Product>)    → a specific closed generic
// GetGenericTypeDefinition() → converts closed → open (for comparison)
// GetGenericArguments()[0]   → gets what T actually is
//
// This is how DbContext automatically finds Products and Orders
// on AppDbContext without you telling it explicitly.
