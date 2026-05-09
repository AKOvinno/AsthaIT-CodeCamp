// ================================================================
//  04 — Custom Attributes and GetCustomAttribute<T>()
//  Attributes are labels you put on classes or properties.
//  Reflection lets you READ those labels at runtime.
// ================================================================

using System.Reflection;
using ReflectionLab.Models;

namespace ReflectionLab.Concepts;

public static class Concept04_Attributes
{
    public static void Run()
    {
        Console.WriteLine("=== 04: Custom Attributes ===\n");

        Type type = typeof(Product);

        // ── Read [Table] from the class ───────────────────────────
        TableAttribute? tableAttr = type.GetCustomAttribute<TableAttribute>();
        Console.WriteLine($"Table name: {tableAttr?.Name}");   // products

        // ── Read [Column] and [PrimaryKey] from each property ─────
        Console.WriteLine("\n-- Scanning properties for attributes --");

        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            bool isPk      = prop.GetCustomAttribute<PrimaryKeyAttribute>() != null;
            ColumnAttribute? colAttr = prop.GetCustomAttribute<ColumnAttribute>();

            if (isPk)
                Console.WriteLine($"  {prop.Name,-15} → [PrimaryKey]");
            else if (colAttr != null)
                Console.WriteLine($"  {prop.Name,-15} → [Column(\"{colAttr.Name}\")]");
            else
                Console.WriteLine($"  {prop.Name,-15} → (no attribute — SKIPPED by ORM)");
        }

        Console.WriteLine();
    }
}

// ── Key takeaway ─────────────────────────────────────────────────
// GetCustomAttribute<T>() returns the attribute instance if found,
// or null if the attribute is not on that class/property.
//
// In MiniOrm, EntityMetadata<T> uses this to:
//   1. Get the table name from [Table("products")]
//   2. Find which property is [PrimaryKey]
//   3. Get the column name from [Column("name")]
//   4. Skip any property with no attribute (navigation props)
