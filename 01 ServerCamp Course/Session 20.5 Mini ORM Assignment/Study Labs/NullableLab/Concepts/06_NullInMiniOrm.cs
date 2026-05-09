// ================================================================
//  06 — Null in MiniOrm End to End
//
//  This file traces exactly how null flows through MiniOrm —
//  from your C# entity, through SQL parameters, into Postgres,
//  and back out again into your entity.
// ================================================================

namespace NullableLab.Concepts;

public static class Concept06_NullInMiniOrm
{
    public static void Run()
    {
        Console.WriteLine("=== 06: Null in MiniOrm End to End ===\n");

        // ── Scenario: Insert a Product with null Discount ─────────
        Console.WriteLine("-- Step 1: You create a Product with null Discount --");
        var product = new ProductFull
        {
            Name     = "Keyboard",
            Price    = 89.99m,
            Discount = null,    // no discount
            InStock  = true
        };
        Console.WriteLine($"product.Discount = {product.Discount?.ToString() ?? "null"}");

        Console.WriteLine();

        // ── What DbSet.AddParams() does ───────────────────────────
        Console.WriteLine("-- Step 2: DbSet.AddParams() converts for Npgsql --");
        var properties = typeof(ProductFull).GetProperties();

        foreach (var prop in properties)
        {
            object? value   = prop.GetValue(product);
            object  dbParam = value ?? DBNull.Value;    // THE key conversion
            Console.WriteLine($"  {prop.Name,-10} C# value: {value?.ToString() ?? "null",-8} → DB param: {dbParam}");
        }

        Console.WriteLine();

        // ── What the SQL looks like ───────────────────────────────
        Console.WriteLine("-- Step 3: The SQL MiniOrm sends to Postgres --");
        Console.WriteLine("  INSERT INTO products (name, price, discount, in_stock)");
        Console.WriteLine("  VALUES (@p0, @p1, @p2, @p3)");
        Console.WriteLine("  @p0 = 'Keyboard'");
        Console.WriteLine("  @p1 = 89.99");
        Console.WriteLine("  @p2 = DBNull.Value  ← Postgres stores this as NULL");
        Console.WriteLine("  @p3 = true");

        Console.WriteLine();

        // ── What Postgres stores ──────────────────────────────────
        Console.WriteLine("-- Step 4: What Postgres stores --");
        Console.WriteLine("  | id | name     | price | discount | in_stock |");
        Console.WriteLine("  |  1 | Keyboard | 89.99 |   NULL   |   true   |");

        Console.WriteLine();

        // ── Reading back: What DbSet.Map() does ───────────────────
        Console.WriteLine("-- Step 5: DbSet.Map() reads the row back --");

        // Simulating what NpgsqlDataReader returns
        var fakeRow = new Dictionary<string, object>
        {
            { "id",       1            },
            { "name",     "Keyboard"   },
            { "price",    89.99m       },
            { "discount", DBNull.Value },   // NULL from Postgres
            { "in_stock", true         }
        };

        var mapped = new ProductFull();

        foreach (var prop in typeof(ProductFull).GetProperties())
        {
            string colName = prop.Name.ToLower();   // simplified mapping
            if (!fakeRow.ContainsKey(colName)) continue;

            object raw = fakeRow[colName];

            // THE key conversion: DBNull.Value → null
            object? finalValue = raw == DBNull.Value ? null : raw;
            prop.SetValue(mapped, finalValue);

            Console.WriteLine($"  {prop.Name,-10} raw: {raw,-12} → C# value: {finalValue?.ToString() ?? "null"}");
        }

        Console.WriteLine();
        Console.WriteLine($"mapped.Discount = {mapped.Discount?.ToString() ?? "null"}");  // null ✓

        Console.WriteLine();

        // ── The operators used throughout ─────────────────────────
        Console.WriteLine("-- Null operators used in MiniOrm --");
        Console.WriteLine("  value ?? DBNull.Value         → null to DBNull when sending");
        Console.WriteLine("  raw == DBNull.Value ? null : raw → DBNull to null when reading");
        Console.WriteLine("  found?.Name                   → safe property access");
        Console.WriteLine("  ?? throw new Exception(...)   → fail fast if config missing");
        Console.WriteLine("  = null!                       → suppress warning for lazy init");
        Console.WriteLine();
    }
}

public class ProductFull
{
    public int      Id       { get; set; }
    public string   Name     { get; set; } = string.Empty;
    public decimal  Price    { get; set; }
    public decimal? Discount { get; set; }
    public bool     InStock  { get; set; }
}
