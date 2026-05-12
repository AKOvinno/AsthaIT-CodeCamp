// ================================================================
//  06 — string.Join (Combine a list into one string)
//
//  string.Join is not technically LINQ, but it works hand-in-hand
//  with LINQ in MiniOrm to build SQL strings from lists.
//
//  It takes a separator and a collection and joins them all
//  into one string with that separator between each item.
//
//  MiniOrm uses string.Join to build:
//    - Column list:  "name, price, discount"
//    - Param list:   "@p0, @p1, @p2"
//    - SET clause:   "name = @p0, price = @p1"
// ================================================================

namespace LinqLab.Concepts;

public static class Concept06_StringJoin
{
    public static void Run()
    {
        Console.WriteLine("=== 06: string.Join ===\n");

        // ── Basic string.Join ─────────────────────────────────────
        var items = new List<string> { "apple", "banana", "cherry" };

        Console.WriteLine("-- Basic string.Join --");
        Console.WriteLine(string.Join(", ", items));    // apple, banana, cherry
        Console.WriteLine(string.Join(" | ", items));   // apple | banana | cherry
        Console.WriteLine(string.Join("\n", items));    // one per line
        Console.WriteLine();

        // ── string.Join + Select ──────────────────────────────────
        // This is the core pattern MiniOrm uses everywhere.
        // Select transforms, then Join combines.

        var columnNames = new List<string> { "name", "price", "discount", "in_stock" };

        Console.WriteLine("-- string.Join + Select --");

        // Column list for INSERT / SELECT
        string colList = string.Join(", ", columnNames);
        Console.WriteLine($"Column list : {colList}");

        // Parameter placeholders for VALUES (...)
        string paramList = string.Join(", ", columnNames.Select((_, i) => $"@p{i}"));
        Console.WriteLine($"Param list  : {paramList}");

        // SET clause for UPDATE
        string setClause = string.Join(", ", columnNames.Select((n, i) => $"{n} = @p{i}"));
        Console.WriteLine($"SET clause  : {setClause}");

        Console.WriteLine();

        // ── Building complete SQL strings ─────────────────────────
        Console.WriteLine("-- Complete SQL strings built by MiniOrm --");
        string tableName = "products";
        string pkColumn  = "id";

        string insertSql = $"INSERT INTO {tableName} ({colList}) VALUES ({paramList}) RETURNING {pkColumn}";
        Console.WriteLine($"INSERT: {insertSql}");
        Console.WriteLine();

        string updateSql = $"UPDATE {tableName} SET {setClause} WHERE {pkColumn} = @pkVal";
        Console.WriteLine($"UPDATE: {updateSql}");
        Console.WriteLine();

        string selectSql = $"SELECT * FROM {tableName}";
        Console.WriteLine($"SELECT: {selectSql}");
        Console.WriteLine();

        string deleteSql = $"DELETE FROM {tableName} WHERE {pkColumn} = @id";
        Console.WriteLine($"DELETE: {deleteSql}");
        Console.WriteLine();

        // ── string.Join with LINQ result values ───────────────────
        Console.WriteLine("-- Joining property values for logging --");
        var values = new object?[] { "Keyboard", 89.99m, null, true };
        string valueLog = string.Join(", ", values.Select(v => v?.ToString() ?? "NULL"));
        Console.WriteLine($"Log: {valueLog}");
        Console.WriteLine();
    }
}
