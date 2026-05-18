// ================================================================
//  01 — What is null and why does it matter?
//
//  null means "no value" or "nothing here".
//  It is one of the most common sources of crashes in C#.
//  MiniOrm deals with null constantly — nullable columns,
//  missing rows, optional parameters.
// ================================================================

namespace NullableLab.Concepts;

public static class Concept01_WhatIsNull
{
    public static void Run()
    {
        Console.WriteLine("=== 01: What is null? ===\n");

        // ── Reference types can be null by default ────────────────
        // string is a reference type — it can hold null
        string? name = "ovinno";
        Console.WriteLine($"name is null: {name == null}");  // True

        // Accessing a property on null crashes with
        // NullReferenceException — the most common C# crash
        try
        {
            // Here, ! operator is null-forgiving operator / dammit operator
            int length = name!.Length;  // CRASH
            // When you have Nullable Reference Types enabled in your project, the compiler tracks whether a variable might be null. If the compiler thinks name could be null, it will show a warning when you try to access .Length. By adding the !, you are manually overriding that warning. Using ! we are telling compilar its not null.
            Console.WriteLine($"name.Length = {length}");
        }
        catch (NullReferenceException)
        {
            Console.WriteLine("Crashed! Cannot call .Length on null");
        }

        Console.WriteLine();

        // ── Value types CANNOT be null by default ─────────────────
        // int, bool, decimal are value types — they always have a value
        int age = 0;       // 0, not null
        bool flag = false; // false, not null

        Console.WriteLine($"int default:  {age}");    // 0
        Console.WriteLine($"bool default: {flag}");   // False

        Console.WriteLine();

        // ── Where MiniOrm encounters null ─────────────────────────
        // 1. Nullable DB columns  → decimal? Discount
        // 2. FindById returns null when row not found
        // 3. DBNull.Value from Npgsql when a column is NULL in DB
        // 4. GetCustomAttribute returns null if attribute not present
        // 5. Nullable.GetUnderlyingType returns null for non-nullable types

        Console.WriteLine("-- MiniOrm null scenarios --");
        Console.WriteLine("decimal? Discount = null   → no discount applied");
        Console.WriteLine("FindById(99)       = null   → row does not exist");
        Console.WriteLine("DBNull.Value               → NULL value from PostgreSQL");
        Console.WriteLine("GetCustomAttribute = null   → attribute not on property");
        Console.WriteLine();
    }
}
