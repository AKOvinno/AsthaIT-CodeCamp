// ================================================================
//  07 — Lambda Expressions
//
//  Lambdas are the short anonymous functions you pass to LINQ.
//  The => symbol is read as "goes to" or "such that".
//
//  p => p.Name        "p such that: give me p.Name"
//  n => n % 2 == 0    "n such that: n is even"
//  (_, i) => $"@p{i}" "ignore first, use index i"
//
//  Every LINQ method in MiniOrm uses lambdas.
// ================================================================

namespace LinqLab.Concepts;

public static class Concept07_LambdaExpressions
{
    public static void Run()
    {
        Console.WriteLine("=== 07: Lambda Expressions ===\n");

        // ── What is a lambda? ─────────────────────────────────────
        // A lambda is a short anonymous function.
        // Instead of writing a named method, you write it inline.

        Console.WriteLine("-- What is a lambda? --");

        // Without lambda — named method
        var numbers = new List<int> { 1, 2, 3, 4, 5, 6 };
        bool IsEven(int n) => n % 2 == 0;
        var evensNamedMethod = numbers.Where(IsEven).ToList();

        // With lambda — inline
        var evensLambda = numbers.Where(n => n % 2 == 0).ToList();

        Console.WriteLine($"Named method result: {string.Join(", ", evensNamedMethod)}");
        Console.WriteLine($"Lambda result:       {string.Join(", ", evensLambda)}");
        Console.WriteLine("Both produce the same result.");
        Console.WriteLine();

        // ── Reading lambda syntax ─────────────────────────────────
        Console.WriteLine("-- Lambda syntax breakdown --");
        Console.WriteLine();
        Console.WriteLine("  n => n % 2 == 0");
        Console.WriteLine("  │    └─────────── body: what to return/do");
        Console.WriteLine("  └─── parameter: the current item");
        Console.WriteLine();
        Console.WriteLine("  (n, i) => $\"@p{i}\"");
        Console.WriteLine("  │  │      └────────── body: format string using index");
        Console.WriteLine("  │  └──── second parameter (index)");
        Console.WriteLine("  └─── first parameter (item)");
        Console.WriteLine();
        Console.WriteLine("  (_, i) => $\"@p{i}\"");
        Console.WriteLine("  │  │      └────────── body");
        Console.WriteLine("  │  └──── index i (used)");
        Console.WriteLine("  └─── _ means: item ignored");
        Console.WriteLine();

        // ── Every lambda from MiniOrm explained ───────────────────
        Console.WriteLine("-- Every lambda used in MiniOrm --");
        Console.WriteLine();

        var columnNames = new List<string> { "name", "price", "discount" };

        // 1. Select with index — build param placeholders
        Console.WriteLine("1. columnNames.Select((_, i) => $\"@p{i}\")");
        var params1 = columnNames.Select((_, i) => $"@p{i}");
        Console.WriteLine($"   → {string.Join(", ", params1)}");
        Console.WriteLine();

        // 2. Select with name and index — build SET clause
        Console.WriteLine("2. columnNames.Select((n, i) => $\"{n} = @p{i}\")");
        var setClause = columnNames.Select((n, i) => $"{n} = @p{i}");
        Console.WriteLine($"   → {string.Join(", ", setClause)}");
        Console.WriteLine();

        // 3. Where — filter pending migrations
        var files   = new List<string> { "20240101_A.sql", "20240102_B.sql", "20240103_C.sql" };
        var applied = new HashSet<string> { "20240101_A.sql" };

        Console.WriteLine("3. files.Where(f => !applied.Contains(Path.GetFileName(f)))");
        var pending = files.Where(f => !applied.Contains(Path.GetFileName(f)));
        Console.WriteLine($"   → {string.Join(", ", pending.Select(Path.GetFileName))}");
        Console.WriteLine();

        // 4. OrderBy — sort by filename
        Console.WriteLine("4. files.OrderBy(f => f)");
        var ordered = files.OrderBy(f => f);
        Console.WriteLine($"   → {string.Join(", ", ordered.Select(Path.GetFileName))}");
        Console.WriteLine();

        // 5. FirstOrDefault — find property with attribute
        Console.WriteLine("5. props.FirstOrDefault(p => p.GetCustomAttribute<T>() != null)");
        Console.WriteLine("   → finds the [PrimaryKey] property in EntityMetadata<T>");
        Console.WriteLine();
    }
}
