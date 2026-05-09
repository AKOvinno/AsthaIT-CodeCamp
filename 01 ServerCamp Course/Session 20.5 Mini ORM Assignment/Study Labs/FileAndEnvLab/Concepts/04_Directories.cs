// ================================================================
//  04 — Directories
//
//  MigrationRunner scans the Migrations/ folder for .sql files.
//  This covers Directory.CreateDirectory, GetFiles, Exists.
// ================================================================

namespace FileAndEnvLab.Concepts;

public static class Concept04_Directories
{
    public static void Run()
    {
        Console.WriteLine("=== 04: Directories ===\n");

        // ── CreateDirectory — creates folder if it doesn't exist ──
        // Safe to call even if the folder already exists — no error
        Directory.CreateDirectory("SampleFiles/Migrations");
        Console.WriteLine("Created: SampleFiles/Migrations/");

        Console.WriteLine();

        // ── Seeding some .sql files to scan ───────────────────────
        File.WriteAllText("SampleFiles/Migrations/20240101_CreateProducts.sql", "-- up\nCREATE TABLE products();\n-- down\nDROP TABLE products;");
        File.WriteAllText("SampleFiles/Migrations/20240102_CreateOrders.sql",   "-- up\nCREATE TABLE orders();\n-- down\nDROP TABLE orders;");
        File.WriteAllText("SampleFiles/Migrations/20240103_AddDiscount.sql",    "-- up\nALTER TABLE products ADD COLUMN discount NUMERIC;\n-- down\nALTER TABLE products DROP COLUMN discount;");

        // ── GetFiles — lists all files matching a pattern ─────────
        // This is exactly what MigrationRunner.GetMigrationFiles() does:
        // Directory.GetFiles(MigrationsFolder, "*.sql")

        Console.WriteLine("-- Directory.GetFiles(\"*.sql\") --");
        string[] sqlFiles = Directory.GetFiles("SampleFiles/Migrations", "*.sql");
        foreach (var file in sqlFiles)
            Console.WriteLine($"  {file}");

        Console.WriteLine();

        // ── OrderBy on filenames ───────────────────────────────────
        // MigrationRunner sorts by filename so migrations apply
        // in the correct chronological order (oldest first)
        Console.WriteLine("-- Sorted by filename (chronological order) --");
        var sorted = sqlFiles.OrderBy(f => f).ToList();
        foreach (var file in sorted)
            Console.WriteLine($"  {Path.GetFileName(file)}");

        Console.WriteLine();

        // ── Directory.Exists ──────────────────────────────────────
        Console.WriteLine("-- Directory.Exists() --");
        Console.WriteLine($"Migrations/ exists:    {Directory.Exists("SampleFiles/Migrations")}");
        Console.WriteLine($"NonExistent/ exists:   {Directory.Exists("NonExistent")}");

        // MigrationRunner uses this pattern:
        // Directory.Exists(MigrationsFolder)
        //     ? Directory.GetFiles(MigrationsFolder, "*.sql")
        //     : []   ← returns empty array if folder missing

        Console.WriteLine();

        // ── Path helpers ──────────────────────────────────────────
        Console.WriteLine("-- Path helpers --");
        string fullPath = "SampleFiles/Migrations/20240101_CreateProducts.sql";

        Console.WriteLine($"GetFileName : {Path.GetFileName(fullPath)}");   // 20240101_CreateProducts.sql
        Console.WriteLine($"GetExtension: {Path.GetExtension(fullPath)}");  // .sql
        Console.WriteLine($"GetDirectory: {Path.GetDirectoryName(fullPath)}");

        // Path.Combine builds paths safely (handles / vs \ on different OS)
        string combined = Path.Combine("SampleFiles", "Migrations", "new.sql");
        Console.WriteLine($"Path.Combine: {combined}");

        Console.WriteLine();
    }
}
