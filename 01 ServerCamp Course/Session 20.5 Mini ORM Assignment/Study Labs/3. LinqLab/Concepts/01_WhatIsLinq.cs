// ================================================================
//  01 — What is LINQ?
//
//  LINQ = Language Integrated Query
//  It lets you query and transform collections using C# methods.
//
//  MiniOrm uses LINQ in DbSet<T> and MigrationRunner to:
//    - Build SQL strings from lists of column names
//    - Filter pending vs applied migrations
//    - Find specific properties by attribute
//    - Sort migration files chronologically
// ================================================================

namespace LinqLab.Concepts;

public static class Concept01_WhatIsLinq
{
    public static void Run()
    {
        Console.WriteLine("=== 01: What is LINQ? ===\n");

        var products = new List<string> { "Keyboard", "Mouse", "Monitor", "Desk", "Chair" };

        // ── Without LINQ — manual loops ───────────────────────────
        Console.WriteLine("-- Without LINQ (manual loop) --");
        var longNames = new List<string>();
        foreach (var p in products)
            if (p.Length > 5)
                longNames.Add(p);
        Console.WriteLine(string.Join(", ", longNames));

        Console.WriteLine();

        // ── With LINQ — clean and readable ────────────────────────
        Console.WriteLine("-- With LINQ --");
        var longNamesLinq = products.Where(p => p.Length > 5).ToList();
        Console.WriteLine(string.Join(", ", longNamesLinq));

        Console.WriteLine();

        // ── Where MiniOrm uses LINQ ───────────────────────────────
        Console.WriteLine("-- MiniOrm uses LINQ in these places --");
        Console.WriteLine();
        Console.WriteLine("  DbSet.InsertAsync:");
        Console.WriteLine("    string.Join(\", \", _meta.ColumnNames)");
        Console.WriteLine("    → joins [\"name\",\"price\",\"discount\"] into \"name, price, discount\"");
        Console.WriteLine();
        Console.WriteLine("  DbSet.InsertAsync:");
        Console.WriteLine("    _meta.ColumnNames.Select((_, i) => $\"@p{i}\")");
        Console.WriteLine("    → builds [\"@p0\", \"@p1\", \"@p2\"] from column names");
        Console.WriteLine();
        Console.WriteLine("  MigrationRunner.ApplyAsync:");
        Console.WriteLine("    files.Where(f => !applied.Contains(Path.GetFileName(f)))");
        Console.WriteLine("    → filters to only pending migration files");
        Console.WriteLine();
        Console.WriteLine("  MigrationRunner.ApplyAsync:");
        Console.WriteLine("    .OrderBy(f => f)");
        Console.WriteLine("    → sorts files chronologically by filename");
        Console.WriteLine();
        Console.WriteLine("  EntityMetadata:");
        Console.WriteLine("    props.FirstOrDefault(p => p.GetCustomAttribute<PrimaryKeyAttribute>() != null)");
        Console.WriteLine("    → finds the [PrimaryKey] property");
        Console.WriteLine();
    }
}
