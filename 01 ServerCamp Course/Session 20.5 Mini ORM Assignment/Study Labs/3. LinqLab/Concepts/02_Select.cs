// ================================================================
//  02 — Select (Transform each item)
//
//  Select transforms every item in a collection into something else.
//  Think of it as "for each item, give me this instead".
//
//  MiniOrm uses Select to build SQL parameter placeholders:
//    _meta.ColumnNames.Select((_, i) => $"@p{i}")
//    → turns ["name","price","discount"] into ["@p0","@p1","@p2"]
// ================================================================

namespace LinqLab.Concepts;

public static class Concept02_Select
{
    public static void Run()
    {
        Console.WriteLine("=== 02: Select — Transform Each Item ===\n");

        // ── Basic Select ──────────────────────────────────────────
        // Transform each string to uppercase
        var names = new List<string> { "name", "price", "discount", "in_stock" };

        Console.WriteLine("-- Basic Select: lowercase → uppercase --");
        var upper = names.Select(n => n.ToUpper()).ToList();
        Console.WriteLine(string.Join(", ", upper));
        Console.WriteLine();

        // ── Select with index — (item, index) ────────────────────
        // The second parameter gives you the position (0-based index)
        // THIS is the pattern MiniOrm uses for parameter placeholders

        Console.WriteLine("-- Select with index: build @p0, @p1, @p2... --");
        var paramNames = names.Select((name, index) => $"@p{index}").ToList();
        Console.WriteLine(string.Join(", ", paramNames));
        // Output: @p0, @p1, @p2, @p3

        Console.WriteLine();

        // ── The _ convention ─────────────────────────────────────
        // When you don't need the item value, just the index,
        // use _ as the variable name to signal "I'm ignoring this"

        Console.WriteLine("-- Using _ to ignore the item value --");
        var onlyIndexes = names.Select((_, i) => $"@p{i}").ToList();
        Console.WriteLine(string.Join(", ", onlyIndexes));
        // Same result — _ means "I don't need the column name, just i"

        Console.WriteLine();

        // ── Select into a different type ──────────────────────────
        Console.WriteLine("-- Select into a different type --");
        var lengths = names.Select(n => n.Length).ToList();
        Console.WriteLine(string.Join(", ", lengths));  // 4, 5, 8, 8
        Console.WriteLine();

        // ── MiniOrm: building the INSERT SQL parts ────────────────
        Console.WriteLine("-- MiniOrm: building INSERT SQL --");
        var columnNames = new List<string> { "name", "price", "discount", "in_stock" };

        // Column list: "name, price, discount, in_stock"
        string colList = string.Join(", ", columnNames);
        Console.WriteLine($"Column list: {colList}");

        // Parameter list: "@p0, @p1, @p2, @p3"
        string paramList = string.Join(", ", columnNames.Select((_, i) => $"@p{i}"));
        Console.WriteLine($"Param list:  {paramList}");

        // Full INSERT SQL:
        string sql = $"INSERT INTO products ({colList}) VALUES ({paramList}) RETURNING id";
        Console.WriteLine($"SQL: {sql}");
        Console.WriteLine();

        // ── MiniOrm: building the SET clause for UPDATE ───────────
        Console.WriteLine("-- MiniOrm: building UPDATE SET clause --");
        string setClause = string.Join(", ", columnNames.Select((n, i) => $"{n} = @p{i}"));
        Console.WriteLine($"SET clause: {setClause}");
        // name = @p0, price = @p1, discount = @p2, in_stock = @p3
        Console.WriteLine();
    }
}
