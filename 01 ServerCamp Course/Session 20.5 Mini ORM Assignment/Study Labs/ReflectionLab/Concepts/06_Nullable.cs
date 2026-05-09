// ================================================================
//  06 — Detecting Nullable Types
//  C# has TWO kinds of nullable — they need different tools.
//
//    int?    → nullable VALUE type   → use Nullable.GetUnderlyingType()
//    string? → nullable REFERENCE type → use NullabilityInfoContext
// ================================================================

using System.Reflection;

namespace ReflectionLab.Concepts;

public static class Concept06_Nullable
{
    public static void Run()
    {
        Console.WriteLine("=== 06: Detecting Nullable Types ===\n");

        // ── Nullable VALUE types ───────────────────────────────────
        // int? is internally Nullable<int> — a struct wrapping the value.
        // Nullable.GetUnderlyingType() unwraps it.

        Type tInt         = typeof(int);
        Type tNullableInt = typeof(int?);
        Type tDecimalNull = typeof(decimal?);
        Type tString      = typeof(string);

        Console.WriteLine("-- Nullable.GetUnderlyingType() --");
        Console.WriteLine($"int         → {Nullable.GetUnderlyingType(tInt)         ?? (object)"null"}");   // null
        Console.WriteLine($"int?        → {Nullable.GetUnderlyingType(tNullableInt) ?? (object)"null"}");   // Int32
        Console.WriteLine($"decimal?    → {Nullable.GetUnderlyingType(tDecimalNull) ?? (object)"null"}");   // Decimal
        Console.WriteLine($"string      → {Nullable.GetUnderlyingType(tString)      ?? (object)"null"}");   // null

        // Rule: if GetUnderlyingType returns non-null → nullable value type
        Type? underlying = Nullable.GetUnderlyingType(tNullableInt);
        if (underlying != null)
            Console.WriteLine($"\nint? wraps: {underlying.Name}");  // Int32

        // ── Nullable REFERENCE types ───────────────────────────────
        // string? is NOT detected by GetUnderlyingType — it returns null for string too.
        // We need NullabilityInfoContext which reads compiler annotations.

        Console.WriteLine("\n-- NullabilityInfoContext for string vs string? --");

        var ctx = new NullabilityInfoContext();

        // Product.Name is string (not nullable)
        PropertyInfo nameProp = typeof(Product).GetProperty("Name")!;
        NullabilityInfo nameInfo = ctx.Create(nameProp);
        Console.WriteLine($"Product.Name  WriteState: {nameInfo.WriteState}");   // NotNull

        // Order.Note is string? (nullable)
        PropertyInfo noteProp = typeof(Order).GetProperty("Note")!;
        NullabilityInfo noteInfo = ctx.Create(noteProp);
        Console.WriteLine($"Order.Note    WriteState: {noteInfo.WriteState}");   // Nullable

        // ── How TypeMapper uses both ───────────────────────────────
        Console.WriteLine("\n-- Simulating TypeMapper.ToPostgresType() --");

        foreach (var prop in typeof(Product).GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            bool isPk = prop.GetCustomAttribute<ReflectionLab.Models.PrimaryKeyAttribute>() != null;
            if (prop.GetCustomAttribute<ReflectionLab.Models.ColumnAttribute>() == null && !isPk) continue;

            string pgType = GetPgType(prop, isPk, ctx);
            Console.WriteLine($"  {prop.Name,-12} → {pgType}");
        }

        Console.WriteLine();
    }

    private static string GetPgType(PropertyInfo prop, bool isPk, NullabilityInfoContext ctx)
    {
        if (isPk) return "SERIAL PRIMARY KEY";

        Type type       = prop.PropertyType;
        Type? underlying = Nullable.GetUnderlyingType(type);

        // nullable value type: int?, decimal?, bool? etc.
        if (underlying != null)
            return $"{MapBase(underlying)} NULL";

        // string / string?
        if (type == typeof(string))
        {
            var info = ctx.Create(prop);
            return info.WriteState == NullabilityState.Nullable ? "TEXT NULL" : "TEXT NOT NULL";
        }

        return $"{MapBase(type)} NOT NULL";
    }

    private static string MapBase(Type t)
    {
        if (t == typeof(int))      return "INTEGER";
        if (t == typeof(decimal))  return "NUMERIC";
        if (t == typeof(bool))     return "BOOLEAN";
        if (t == typeof(string))   return "TEXT";
        if (t == typeof(DateTime)) return "TIMESTAMP";
        return "TEXT";
    }
}

// ── Key takeaway ─────────────────────────────────────────────────
// Nullable.GetUnderlyingType()  → detects int?, decimal?, bool? etc.
// NullabilityInfoContext        → detects string? vs string
// These two together cover ALL nullable types in TypeMapper.
