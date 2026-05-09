// ================================================================
//  02 — GetProperties() and PropertyInfo
//  PropertyInfo represents ONE property of a class.
//  GetProperties() returns ALL of them as an array.
// ================================================================

using System.Reflection;

namespace ReflectionLab.Concepts;

public static class Concept02_GetProperties
{
    public static void Run()
    {
        Console.WriteLine("=== 02: GetProperties() and PropertyInfo ===\n");

        Type type = typeof(Product);

        // BindingFlags control WHICH properties you get back.
        // Public   = only public properties
        // Instance = only non-static properties
        // | combines both conditions (like AND)
        PropertyInfo[] props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var prop in props)
        {
            Console.WriteLine($"  {prop.Name,-15} → {prop.PropertyType.Name}");
        }

        // Output:
        //   Id              → Int32
        //   Name            → String
        //   Price           → Decimal
        //   Discount        → Nullable`1   ← this is decimal?
        //   InStock         → Boolean
        //   IgnoredProp     → String

        Console.WriteLine();
        Console.WriteLine("-- Nullable`1 means this is a nullable value type (decimal?) --");
        Console.WriteLine("-- The ORM skips IgnoredProp because it has no [Column] attr --");
        Console.WriteLine();
    }
}

// ── Key takeaway ─────────────────────────────────────────────────
// In MiniOrm, EntityMetadata<T> calls GetProperties() to scan
// the Product class and find all its Id, Name, Price etc.
// It then checks each one for [PrimaryKey] or [Column] attributes
// and skips any that have neither.
