// ================================================================
//  08 — LINQ in MiniOrm End to End
//
//  This file traces every LINQ call in MiniOrm — showing the
//  input, the LINQ operation, and the output — so you can
//  map each line of MiniOrm code to a concrete example.
// ================================================================

using System.Reflection;

namespace LinqLab.Concepts;

public static class Concept08_LinqInMiniOrm
{
    public static void Run()
    {
        Console.WriteLine("=== 08: LINQ in MiniOrm End to End ===\n");

        // ── DbSet<T>.InsertAsync ──────────────────────────────────
        Console.WriteLine("=== DbSet<T>.InsertAsync ===\n");

        var columnNames = new List<string> { "name", "price", "discount", "in_stock" };
        string tableName = "products";
        string pkColumn  = "id";

        // Line: string.Join(", ", _meta.ColumnNames)
        string colList = string.Join(", ", columnNames);
        Console.WriteLine($"ColumnNames  → \"{colList}\"");

        // Line: string.Join(", ", _meta.ColumnNames.Select((_, i) => $"@p{i}"))
        string paramList = string.Join(", ", columnNames.Select((_, i) => $"@p{i}"));
        Console.WriteLine($"ParamList    → \"{paramList}\"");

        string insertSql = $"INSERT INTO {tableName} ({colList}) VALUES ({paramList}) RETURNING {pkColumn}";
        Console.WriteLine($"INSERT SQL   → {insertSql}");
        Console.WriteLine();

        // ── DbSet<T>.UpdateAsync ──────────────────────────────────
        Console.WriteLine("=== DbSet<T>.UpdateAsync ===\n");

        // Line: string.Join(", ", _meta.ColumnNames.Select((n, i) => $"{n} = @p{i}"))
        string setClause = string.Join(", ", columnNames.Select((n, i) => $"{n} = @p{i}"));
        Console.WriteLine($"SET clause   → \"{setClause}\"");

        string updateSql = $"UPDATE {tableName} SET {setClause} WHERE {pkColumn} = @pkVal";
        Console.WriteLine($"UPDATE SQL   → {updateSql}");
        Console.WriteLine();

        // ── DbSet<T>.AddParams ────────────────────────────────────
        Console.WriteLine("=== DbSet<T>.AddParams — reading values with GetValue ===\n");

        // In MiniOrm: _meta.Columns[i].GetValue(entity)
        // Simulating with a fake product
        var product = new LinqProduct { Name = "Keyboard", Price = 89.99m, Discount = null, InStock = true };

        var props = typeof(LinqProduct).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.Name != "Id")   // skip PK
            .ToList();

        Console.WriteLine("Property values sent as SQL params:");
        for (int i = 0; i < props.Count; i++)
        {
            object? value   = props[i].GetValue(product);
            object  dbParam = value ?? DBNull.Value;
            Console.WriteLine($"  @p{i} ({props[i].Name}) = {dbParam}");
        }
        Console.WriteLine();

        // ── EntityMetadata<T> — finding the PK property ──────────
        Console.WriteLine("=== EntityMetadata<T> — FirstOrDefault for PK ===\n");

        var allProps = typeof(LinqProduct).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        // Line: props.FirstOrDefault(p => p.GetCustomAttribute<PrimaryKeyAttribute>() != null)
        var pkProp = allProps.FirstOrDefault(
            p => p.GetCustomAttribute<LinqPrimaryKeyAttribute>() != null)
            ?? throw new InvalidOperationException("No [PrimaryKey] found");

        Console.WriteLine($"PK property: {pkProp.Name}");
        Console.WriteLine();

        // ── MigrationRunner.ApplyAsync ────────────────────────────
        Console.WriteLine("=== MigrationRunner.ApplyAsync — Where + OrderBy + ToList ===\n");

        var allFiles = new List<string>
        {
            "Migrations/20240103_AddOrders.sql",
            "Migrations/20240101_InitialCreate.sql",
            "Migrations/20240102_AddDiscount.sql"
        };

        var appliedMigrations = new HashSet<string>
        {
            "20240101_InitialCreate.sql"
        };

        // Line: GetMigrationFiles().Where(...).OrderBy(...).ToList()
        var pending = allFiles
            .Where(f => !appliedMigrations.Contains(Path.GetFileName(f)))
            .OrderBy(f => f)
            .ToList();

        Console.WriteLine("Pending migrations (filtered + sorted):");
        pending.ForEach(f => Console.WriteLine($"  {Path.GetFileName(f)}"));
        Console.WriteLine();

        // ── MigrationRunner.ListAsync ─────────────────────────────
        Console.WriteLine("=== MigrationRunner.ListAsync — applied.Contains ===\n");

        foreach (var file in allFiles.OrderBy(f => f))
        {
            string name   = Path.GetFileName(file);
            // Line: applied.Contains(name) ? "[applied]" : "[pending]"
            string status = appliedMigrations.Contains(name) ? "[applied]" : "[pending]";
            Console.WriteLine($"  {status} {name}");
        }
        Console.WriteLine();
    }
}

// ── Mini models for this file ────────────────────────────────────
[AttributeUsage(AttributeTargets.Property)]
public sealed class LinqPrimaryKeyAttribute : Attribute { }

public class LinqProduct
{
    [LinqPrimaryKey] public int      Id       { get; set; }
    public string   Name     { get; set; } = string.Empty;
    public decimal  Price    { get; set; }
    public decimal? Discount { get; set; }
    public bool     InStock  { get; set; }
}
