// ================================================================
//  02 — Nullable Value Types  (int?, decimal?, bool?)
//
//  Value types (int, bool, decimal) cannot be null normally.
//  Adding ? makes them nullable — wrapping them in Nullable<T>.
//
//  In MiniOrm: decimal? Discount maps to NUMERIC NULL in Postgres.
// ================================================================

namespace NullableLab.Concepts;

public static class Concept02_NullableValueTypes
{
    public static void Run()
    {
        Console.WriteLine("=== 02: Nullable Value Types ===\n");

        // ── Declaring nullable value types ────────────────────────
        int     regularInt  = 5;      // cannot be null
        int?    nullableInt = null;    // CAN be null
        decimal? discount   = null;   // MiniOrm: decimal? Discount

        Console.WriteLine($"regularInt  = {regularInt}");
        Console.WriteLine($"nullableInt = {nullableInt ?? (object)"null"}");
        Console.WriteLine($"discount    = {discount    ?? (object)"null"}");

        Console.WriteLine();

        // ── .HasValue and .Value ──────────────────────────────────
        // Nullable<T> has two properties: HasValue and Value
        int? a = 42;
        int? b = null;

        Console.WriteLine($"a.HasValue = {a.HasValue}");   // True
        Console.WriteLine($"a.Value    = {a.Value}");      // 42
        Console.WriteLine($"b.HasValue = {b.HasValue}");   // False

        // Accessing .Value when HasValue is false → crash!
        try
        {
            var crash = b!.Value;
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine("Crashed! Cannot get .Value when null");
        }

        Console.WriteLine();

        // ── Safe ways to read a nullable value ───────────────────
        int? score = null;

        // Option 1: check HasValue first
        if (score.HasValue)
            Console.WriteLine($"Score: {score.Value}");
        else
            Console.WriteLine("Score: not set");

        // Option 2: use ?? (null coalescing) — covered in 04
        Console.WriteLine($"Score with default: {score ?? 0}");

        Console.WriteLine();

        // ── How MiniOrm uses nullable value types ─────────────────
        Console.WriteLine("-- MiniOrm connection --");
        var product = new ProductDemo { Name = "Keyboard", Discount = null };

        // When inserting — null becomes DBNull.Value for Npgsql
        object dbValue = product.Discount.HasValue
            ? (object)product.Discount.Value
            : DBNull.Value;
        Console.WriteLine($"Discount sent to DB: {dbValue}");  // DBNull.Value

        product.Discount = 5.00m;
        dbValue = product.Discount.HasValue
            ? (object)product.Discount.Value
            : DBNull.Value;
        Console.WriteLine($"Discount sent to DB: {dbValue}");  // 5.00

        // In DbSet.AddParams() this is simplified to:
        // cmd.Parameters.AddWithValue("p0", value ?? DBNull.Value)
        Console.WriteLine();
    }
}

public class ProductDemo
{
    public string   Name     { get; set; } = string.Empty;
    public decimal? Discount { get; set; }  // nullable — maps to NUMERIC NULL
}
