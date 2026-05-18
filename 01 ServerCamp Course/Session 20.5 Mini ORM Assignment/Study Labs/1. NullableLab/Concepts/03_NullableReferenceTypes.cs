// ================================================================
//  03 — Nullable Reference Types  (string?)
//
//  In modern C# (with <Nullable>enable</Nullable> in .csproj),
//  string is NOT nullable by default.
//  string? explicitly says "this can be null".
//
//  In MiniOrm: string? Note maps to TEXT NULL in Postgres.
// ================================================================

namespace NullableLab.Concepts;

public static class Concept03_NullableReferenceTypes
{
    public static void Run()
    {
        Console.WriteLine("=== 03: Nullable Reference Types ===\n");

        // ── string vs string? ─────────────────────────────────────
        string  required = "Hello";   // compiler warns if you assign null
        string? optional = "ovinno";      // explicitly allowed to be null

        Console.WriteLine($"required = {required}");
        Console.WriteLine($"optional = {optional ?? "null"}");

        Console.WriteLine();

        // ── Why this matters in MiniOrm ───────────────────────────
        // Order.Note is string? — the column can be NULL in Postgres
        // Product.Name is string — the column must NOT be NULL

        var order = new OrderDemo { Item = "Monitor", Note = null };
        Console.WriteLine($"Note is null: {order.Note == null}");  // True

        // When reading from DB — null column becomes null in C#
        string? noteFromDb = null;  // simulating DBNull read
        order.Note = noteFromDb;
        Console.WriteLine($"Note after DB read: {order.Note ?? "NULL"}");

        Console.WriteLine();

        // ── The compiler helps you ────────────────────────────────
        // With Nullable enabled, the compiler WARNS you when you
        // might accidentally use a null value.

        string? maybeNull = GetMaybeName();

        // Without null check — compiler warns: "maybeNull may be null"
        // Console.WriteLine(maybeNull.Length);  // ← compiler warning

        // With null check — compiler is satisfied
        if (maybeNull != null)
            Console.WriteLine($"Name length: {maybeNull.Length}");  // safe
        else
            Console.WriteLine("Name is null — skipping length check");

        Console.WriteLine();

        // ── null! initializer ─────────────────────────────────────
        // In MiniOrm you see:   public DbSet<Product> Products { get; set; } = null!;
        // The ! tells compiler: "I know this looks null now but trust me,
        // it will be set before anyone uses it"
        // (DbContext.InitializeSets() sets it immediately in the constructor)

        Console.WriteLine("-- null! explained --");
        Console.WriteLine("public DbSet<Product> Products { get; set; } = null!;");
        Console.WriteLine("The ! suppresses the compiler warning.");
        Console.WriteLine("DbContext sets it in the constructor before it's ever used.");
        Console.WriteLine();
    }

    static string? GetMaybeName()
    {
        // Simulates a method that might return null
        return null;
    }
}

public class OrderDemo
{
    public string  Item { get; set; } = string.Empty;
    public string? Note { get; set; }  // nullable — maps to TEXT NULL
}
