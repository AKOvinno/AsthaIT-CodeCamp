// ================================================================
//  06 — default(T) and typeof(T) inside generics
//
//  Inside a generic class you often need:
//    default(T) → the zero/null value of T
//    typeof(T)  → the Type object of T at runtime
//
//  These are used heavily in MiniOrm's DbSet<T>.
// ================================================================

using GenericsLab.Models;

namespace GenericsLab.Concepts;

public static class Concept06_DefaultAndTypeof
{
    public static void Run()
    {
        Console.WriteLine("=== 06: default(T) and typeof(T) ===\n");

        // ── default(T) ────────────────────────────────────────────
        // Returns the "zero value" of T:
        //   int     → 0
        //   bool    → false
        //   string  → null
        //   Product → null  (reference types default to null)

        Console.WriteLine("-- default(T) values --");
        Console.WriteLine($"default(int)     = {default(int)}");       // 0
        Console.WriteLine($"default(bool)    = {default(bool)}");      // False
        Console.WriteLine($"default(string)  = {default(string) ?? "null"}");   // null
        Console.WriteLine($"default(Product) = {default(Product) ?? (object)"null"}"); // null

        // In a generic method:
        Console.WriteLine($"GetDefault<int>()     = {GetDefault<int>()}");
        Console.WriteLine($"GetDefault<bool>()    = {GetDefault<bool>()}");
        Console.WriteLine($"GetDefault<Product>() = {GetDefault<Product>() ?? (object)"null"}");

        Console.WriteLine();

        // ── How MiniOrm uses default ──────────────────────────────
        // In DbSet<T>.FindByIdAsync(), if no row is found:
        //   return default;
        // For Product (a class), this returns null.
        // This is why FindByIdAsync returns T? (nullable T).

        Console.WriteLine("-- MiniOrm connection: FindByIdAsync returns default when not found --");
        var result = SimulatedFindById<Product>(99);  // not found
        Console.WriteLine($"FindById(99) = {result ?? (object)"null"}");  // null
        Console.WriteLine();

        // ── typeof(T) inside a generic ────────────────────────────
        // typeof(T) gives you the actual Type of T at runtime.
        // You can then use reflection on it.

        Console.WriteLine("-- typeof(T) inside generics --");
        PrintTypeName<Product>();   // T is Product
        PrintTypeName<Order>();     // T is Order
        PrintTypeName<int>();       // T is Int32

        Console.WriteLine();

        // ── How MiniOrm uses typeof(T) ────────────────────────────
        // In EntityMetadata<T>:
        //   var type = typeof(T);
        //   var props = type.GetProperties(...);
        //   var tableAttr = type.GetCustomAttribute<TableAttribute>();
        //
        // This is how DbSet<Product> knows to scan Product's properties.

        Console.WriteLine("-- MiniOrm connection: typeof(T) powers EntityMetadata<T> --");
        InspectEntity<Product>();
        Console.WriteLine();
    }

    public static T? GetDefault<T>() => default(T);

    public static void PrintTypeName<T>()
    {
        Console.WriteLine($"  T is: {typeof(T).Name}");
    }

    public static T? SimulatedFindById<T>(int id) where T : class
    {
        // Pretend we searched the DB and found nothing
        return default;   // returns null for reference types
    }

    public static void InspectEntity<T>()
    {
        Type type = typeof(T);
        Console.WriteLine($"  Inspecting: {type.Name}");
        foreach (var prop in type.GetProperties())
            Console.WriteLine($"    {prop.Name} ({prop.PropertyType.Name})");
    }
}

// ── Key takeaway ─────────────────────────────────────────────────
// default(T)  → zero/null value for any T. Used in FindByIdAsync
//               to return null when no row is found.
// typeof(T)   → the Type blueprint of T. Used in EntityMetadata<T>
//               to scan properties, read attributes, map columns.
// Both are essential tools inside any generic class.
