// ================================================================
//  04 — FirstOrDefault (Find one item)
//
//  FirstOrDefault returns the FIRST item matching a condition,
//  or null (default) if nothing matches.
//
//  MiniOrm uses FirstOrDefault in EntityMetadata<T> to find
//  the property that has the [PrimaryKey] attribute.
// ================================================================

using System.Reflection;

namespace LinqLab.Concepts;

public static class Concept04_FirstOrDefault
{
    public static void Run()
    {
        Console.WriteLine("=== 04: FirstOrDefault — Find One Item ===\n");

        // ── Basic FirstOrDefault ──────────────────────────────────
        var numbers = new List<int> { 3, 7, 1, 9, 4, 6 };

        Console.WriteLine("-- Basic FirstOrDefault --");
        int first = numbers.First();                          // first item: 3
        int firstEven = numbers.First(n => n % 2 == 0);      // first even: 4

        Console.WriteLine($"First():          {first}");
        Console.WriteLine($"First(even):      {firstEven}");
        Console.WriteLine();

        // ── FirstOrDefault returns null when not found ────────────
        // First() would THROW if nothing matches.
        // FirstOrDefault() returns null (or 0 for int) — much safer.

        Console.WriteLine("-- FirstOrDefault vs First --");
        int? found    = numbers.FirstOrDefault(n => n > 100);  // not found → 0
        Console.WriteLine($"FirstOrDefault(n > 100) = {found}");  // 0 (default int)

        var names = new List<string> { "Keyboard", "Mouse", "Monitor" };
        string? foundName = names.FirstOrDefault(n => n.StartsWith("Z"));
        Console.WriteLine($"FirstOrDefault(starts Z) = {foundName ?? "null"}");  // null

        Console.WriteLine();

        // ── ?? throw pattern ──────────────────────────────────────
        // MiniOrm uses FirstOrDefault combined with ?? throw
        // to crash with a clear message if nothing is found.
        // This is EntityMetadata<T>'s PrimaryKey scan:
        //
        //   PrimaryKey = props.FirstOrDefault(p =>
        //       p.GetCustomAttribute<PrimaryKeyAttribute>() != null)
        //       ?? throw new InvalidOperationException("No [PrimaryKey] found");

        Console.WriteLine("-- FirstOrDefault ?? throw (EntityMetadata pattern) --");

        var mockProps = new List<MockProperty>
        {
            new("Id",       hasPk: true),
            new("Name",     hasPk: false),
            new("Price",    hasPk: false),
        };

        // Find the property with [PrimaryKey]
        var pkProp = mockProps.FirstOrDefault(p => p.HasPrimaryKey)
            ?? throw new InvalidOperationException("No [PrimaryKey] property found!");

        Console.WriteLine($"Found PK property: {pkProp.Name}");

        // What happens when [PrimaryKey] is missing:
        var noPkProps = new List<MockProperty>
        {
            new("Name",  hasPk: false),
            new("Price", hasPk: false),
        };

        try
        {
            var missing = noPkProps.FirstOrDefault(p => p.HasPrimaryKey)
                ?? throw new InvalidOperationException("No [PrimaryKey] property found!");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Caught: {ex.Message}");
        }

        Console.WriteLine();

        // ── FirstOrDefault with reflection (actual MiniOrm code) ─
        Console.WriteLine("-- Real reflection + FirstOrDefault (EntityMetadata style) --");

        var props = typeof(SampleProduct).GetProperties(
            BindingFlags.Public | BindingFlags.Instance);

        // Find the property with [SamplePrimaryKey]
        var primaryKey = props.FirstOrDefault(
            p => p.GetCustomAttribute<SamplePrimaryKeyAttribute>() != null)
            ?? throw new InvalidOperationException($"{nameof(SampleProduct)} has no [PrimaryKey]");

        Console.WriteLine($"PrimaryKey property: {primaryKey.Name} ({primaryKey.PropertyType.Name})");
        Console.WriteLine();
    }
}

// ── Helpers ───────────────────────────────────────────────────────
public record MockProperty(string Name, bool HasPrimaryKey);

[AttributeUsage(AttributeTargets.Property)]
public sealed class SamplePrimaryKeyAttribute : Attribute { }

public class SampleProduct
{
    [SamplePrimaryKey] public int     Id    { get; set; }
    public string  Name  { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
