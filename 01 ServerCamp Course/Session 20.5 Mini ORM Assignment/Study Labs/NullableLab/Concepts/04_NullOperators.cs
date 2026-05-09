// ================================================================
//  04 — Null Operators  (??, ??=, ?., !)
//
//  C# has several operators for dealing with null cleanly.
//  MiniOrm uses all of them. This file explains each one.
// ================================================================

namespace NullableLab.Concepts;

public static class Concept04_NullOperators
{
    public static void Run()
    {
        Console.WriteLine("=== 04: Null Operators ===\n");

        // ── ?? — Null Coalescing Operator ─────────────────────────
        // "If the left side is null, use the right side instead"
        // This is the most used null operator in MiniOrm.

        Console.WriteLine("-- ?? (null coalescing) --");
        string? name = null;
        string result = name ?? "default name";
        Console.WriteLine(result);   // default name

        decimal? discount = null;
        decimal finalDiscount = discount ?? 0m;
        Console.WriteLine($"Discount: {finalDiscount}");  // 0

        // MiniOrm uses this in DbSet.AddParams():
        // cmd.Parameters.AddWithValue("p0", value ?? DBNull.Value)
        // → if value is null, send DBNull.Value to Postgres instead

        object? propValue = null;
        object dbParam = propValue ?? DBNull.Value;
        Console.WriteLine($"DB param: {dbParam}");  // System.DBNull

        Console.WriteLine();

        // ── ?? in throw expressions ───────────────────────────────
        // MiniOrm uses this pattern everywhere:
        // var connStr = Environment.GetEnvironmentVariable("MINIORM_CONN")
        //     ?? throw new InvalidOperationException("not set");
        // → if env var is null, throw immediately

        Console.WriteLine("-- ?? with throw --");
        try
        {
            string? envVar = null;   // simulating missing env var
            string conn = envVar ?? throw new InvalidOperationException("MINIORM_CONN is not set");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Caught: {ex.Message}");
        }

        Console.WriteLine();

        // ── ?. — Null Conditional Operator ───────────────────────
        // "Only call this method/property if the object is not null"
        // Avoids NullReferenceException safely.

        Console.WriteLine("-- ?. (null conditional) --");
        string? maybeString = null;

        // Without ?. this would crash:
        // int len = maybeString.Length;  // NullReferenceException!

        // With ?. it returns null instead of crashing:
        int? len = maybeString?.Length;
        Console.WriteLine($"Length: {len ?? (object)"null"}");  // null

        maybeString = "Hello";
        len = maybeString?.Length;
        Console.WriteLine($"Length: {len}");  // 5

        // MiniOrm uses this in Program.cs:
        // Console.WriteLine(found?.Name);
        // → if found is null, prints nothing instead of crashing

        Console.WriteLine();

        // ── ! — Null Forgiving Operator ───────────────────────────
        // Tells the compiler "I guarantee this is not null"
        // Use it when YOU know something is not null but the
        // compiler cannot figure it out.

        Console.WriteLine("-- ! (null forgiving) --");
        string? possiblyNull = GetName();

        // Compiler doesn't know GetName() always returns a value
        // Without ! → compiler warning: "may be null"
        // With    ! → you take responsibility, no warning
        string definitelyNotNull = possiblyNull!;
        Console.WriteLine(definitelyNotNull);  // Always "Ashfaq"

        // MiniOrm uses ! in several places:
        // var id = (int)(await cmd.ExecuteScalarAsync())!;
        // → we KNOW ExecuteScalar returns an int here (RETURNING id)
        //   so we tell compiler to trust us

        // Also: type.GetProperty("Name")!
        // → we know Product has a Name property

        Console.WriteLine();

        // ── ??= — Null Coalescing Assignment ─────────────────────
        // "Assign this value only if the variable is currently null"

        Console.WriteLine("-- ??= (null coalescing assignment) --");
        string? label = null;
        label ??= "default label";
        Console.WriteLine(label);   // default label

        label ??= "another value";  // label is not null anymore
        Console.WriteLine(label);   // default label (unchanged)

        Console.WriteLine();
    }

    static string? GetName() => "Ashfaq";
}
