// ================================================================
//  05 — OrderBy and ToList
//
//  OrderBy  → sorts a collection by a key
//  ToList() → materialises the LINQ result into a real List<T>
//
//  MiniOrm uses OrderBy in MigrationRunner to ensure migrations
//  apply in the correct chronological order (oldest first).
//  ToList() is used everywhere to turn query results into lists.
// ================================================================

namespace LinqLab.Concepts;

public static class Concept05_OrderByAndToList
{
    public static void Run()
    {
        Console.WriteLine("=== 05: OrderBy and ToList ===\n");

        // ── OrderBy — ascending sort ──────────────────────────────
        var numbers = new List<int> { 5, 2, 8, 1, 9, 3 };

        Console.WriteLine("-- OrderBy (ascending) --");
        var sorted = numbers.OrderBy(n => n).ToList();
        Console.WriteLine(string.Join(", ", sorted));   // 1, 2, 3, 5, 8, 9
        Console.WriteLine();

        // ── OrderByDescending — descending sort ───────────────────
        Console.WriteLine("-- OrderByDescending --");
        var sortedDesc = numbers.OrderByDescending(n => n).ToList();
        Console.WriteLine(string.Join(", ", sortedDesc));  // 9, 8, 5, 3, 2, 1
        Console.WriteLine();

        // ── OrderBy on strings ────────────────────────────────────
        // String sort is alphabetical — which for timestamps is
        // also chronological because of the yyyyMMddHHmmss format.
        // This is exactly why MigrationRunner sorts by filename.

        Console.WriteLine("-- OrderBy on migration filenames (chronological) --");
        var files = new List<string>
        {
            "20240103000000_AddOrders.sql",
            "20240101000000_InitialCreate.sql",   // oldest
            "20240102000000_AddDiscount.sql"
        };

        var chronological = files.OrderBy(f => f).ToList();
        Console.WriteLine("Sorted (oldest first):");
        chronological.ForEach(f => Console.WriteLine($"  {f}"));
        Console.WriteLine();

        // This matters because if you applied AddDiscount before
        // InitialCreate, the products table wouldn't exist yet
        // and the migration would fail.

        // ── ToList() — why it's needed ────────────────────────────
        // LINQ methods return IEnumerable<T>, not List<T>.
        // IEnumerable is "lazy" — it doesn't execute until you
        // iterate it. ToList() forces execution immediately
        // and gives you a real List<T> you can use freely.

        Console.WriteLine("-- Why ToList() is needed --");

        var query = files.Where(f => f.EndsWith(".sql")); // IEnumerable — not executed yet
        Console.WriteLine($"query type: {query.GetType().Name}");  // WhereListIterator

        var list = query.ToList();   // NOW it executes and gives us List<T>
        Console.WriteLine($"list type:  {list.GetType().Name}");   // List`1
        Console.WriteLine($"list count: {list.Count}");
        Console.WriteLine();

        // ── MiniOrm: full pending migrations query ────────────────
        Console.WriteLine("-- MiniOrm: full pending migrations pipeline --");

        var allFiles    = files;
        var applied     = new HashSet<string> { "20240101000000_InitialCreate.sql" };

        // This is the exact LINQ chain in MigrationRunner.ApplyAsync():
        var pending = allFiles
            .Where(f => !applied.Contains(Path.GetFileName(f)))  // filter
            .OrderBy(f => f)                                       // sort
            .ToList();                                             // materialise

        Console.WriteLine("Pending in order:");
        pending.ForEach(f => Console.WriteLine($"  {Path.GetFileName(f)}"));
        Console.WriteLine();
    }
}
