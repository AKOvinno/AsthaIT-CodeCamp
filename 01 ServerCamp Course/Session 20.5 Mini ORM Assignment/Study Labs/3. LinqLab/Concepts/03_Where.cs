// ================================================================
//  03 — Where (Filter items)
//
//  Where filters a collection — keeps only items that match
//  a condition. Think of it as "give me only the ones where...".
//
//  MiniOrm uses Where in MigrationRunner.ApplyAsync() to find
//  only the migration files that haven't been applied yet.
// ================================================================

namespace LinqLab.Concepts;

public static class Concept03_Where
{
    public static void Run()
    {
        Console.WriteLine("=== 03: Where — Filter Items ===\n");

        // ── Basic Where ───────────────────────────────────────────
        var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        Console.WriteLine("-- Basic Where: keep only even numbers --");
        var evens = numbers.Where(n => n % 2 == 0).ToList();
        Console.WriteLine(string.Join(", ", evens));   // 2, 4, 6, 8, 10
        Console.WriteLine();

        // ── Where with string condition ───────────────────────────
        Console.WriteLine("-- Where on strings --");
        var files = new List<string>
        {
            "20240101_InitialCreate.sql",
            "20240102_AddDiscount.sql",
            "20240103_AddOrders.sql",
            "notes.txt",
            "readme.md"
        };

        var sqlFiles = files.Where(f => f.EndsWith(".sql")).ToList();
        Console.WriteLine("Only .sql files:");
        sqlFiles.ForEach(f => Console.WriteLine($"  {f}"));
        Console.WriteLine();

        // ── Where with NOT — filtering out applied migrations ─────
        // This is EXACTLY what MigrationRunner.ApplyAsync() does:
        //   files.Where(f => !applied.Contains(Path.GetFileName(f)))

        Console.WriteLine("-- MiniOrm: filtering pending migrations --");

        var allMigrationFiles = new List<string>
        {
            "LabMigrations/20240101_InitialCreate.sql",
            "LabMigrations/20240102_AddDiscount.sql",
            "LabMigrations/20240103_AddOrders.sql"
        };

        var appliedMigrations = new HashSet<string>
        {
            "20240101_InitialCreate.sql",   // already applied
            "20240102_AddDiscount.sql"       // already applied
        };

        // Filter: keep only files whose filename is NOT in applied set
        var pendingFiles = allMigrationFiles
            .Where(f => !appliedMigrations.Contains(Path.GetFileName(f)))
            .ToList();

        Console.WriteLine("All files:");
        allMigrationFiles.ForEach(f => Console.WriteLine($"  {Path.GetFileName(f)}"));

        Console.WriteLine("\nApplied:");
        appliedMigrations.ToList().ForEach(a => Console.WriteLine($"  {a}"));

        Console.WriteLine("\nPending (after Where filter):");
        pendingFiles.ForEach(f => Console.WriteLine($"  {Path.GetFileName(f)}"));
        Console.WriteLine();

        // ── Chaining Where with other LINQ methods ────────────────
        Console.WriteLine("-- Chaining Where + OrderBy --");
        var pending = allMigrationFiles
            .Where(f => !appliedMigrations.Contains(Path.GetFileName(f)))
            .OrderBy(f => f)   // sort chronologically
            .ToList();

        Console.WriteLine("Pending in order:");
        pending.ForEach(f => Console.WriteLine($"  {Path.GetFileName(f)}"));
        Console.WriteLine();
    }
}
