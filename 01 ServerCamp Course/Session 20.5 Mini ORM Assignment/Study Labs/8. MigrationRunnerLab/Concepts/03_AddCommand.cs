// ================================================================
//  03 — The "migrations add" Command
//
//  "add" generates a timestamped .sql file with -- up and -- down
//  sections. It does NOT touch the database at all.
//
//  This file shows exactly how MigrationRunner.Add() works,
//  step by step, with no database required.
// ================================================================

namespace MigrationRunnerLab.Concepts;

public static class Concept03_AddCommand
{
    public static void Run()
    {
        Console.WriteLine("=== 03: The 'migrations add' Command ===\n");

        // ── Step 1: Build the migration name ──────────────────────
        // The filename is: {timestamp}_{name}.sql
        // Timestamp ensures files sort chronologically.

        string migrationName = "InitialCreate";
        string timestamp     = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        string fileName      = $"{timestamp}_{migrationName}.sql";

        Console.WriteLine($"-- Step 1: Build filename --");
        Console.WriteLine($"Timestamp : {timestamp}");
        Console.WriteLine($"Name      : {migrationName}");
        Console.WriteLine($"FileName  : {fileName}");
        Console.WriteLine();

        // ── Step 2: Build the -- up SQL ───────────────────────────
        // In MigrationRunner, BuildColumnDefs() + BuildCreateTable()
        // generate this SQL from your entity's reflection metadata.
        // Here we hard-code it to focus on the file structure.

        Console.WriteLine("-- Step 2: Build -- up SQL --");
        string upSql =
            "CREATE TABLE IF NOT EXISTS products (\n" +
            "    id       SERIAL PRIMARY KEY,\n" +
            "    name     TEXT NOT NULL,\n" +
            "    price    NUMERIC NOT NULL,\n" +
            "    discount NUMERIC NULL,\n" +
            "    in_stock BOOLEAN NOT NULL\n" +
            ");\n\n" +
            "CREATE TABLE IF NOT EXISTS orders (\n" +
            "    id         SERIAL PRIMARY KEY,\n" +
            "    product_id INTEGER NOT NULL,\n" +
            "    quantity   INTEGER NOT NULL,\n" +
            "    note       TEXT NULL,\n" +
            "    placed_at  TIMESTAMP NOT NULL\n" +
            ");";

        Console.WriteLine(upSql);
        Console.WriteLine();

        // ── Step 3: Build the -- down SQL ─────────────────────────
        // Down reverses the up. DROP TABLE undoes CREATE TABLE.
        // Order is reversed — drop orders before products
        // because orders may reference products.

        Console.WriteLine("-- Step 3: Build -- down SQL --");
        string downSql =
            "DROP TABLE IF EXISTS orders;\n" +
            "DROP TABLE IF EXISTS products;";

        Console.WriteLine(downSql);
        Console.WriteLine();

        // ── Step 4: Combine into the file content ─────────────────
        string fileContent =
            "-- up\n"     + upSql   +
            "\n\n-- down\n" + downSql + "\n";

        Console.WriteLine("-- Step 4: Full file content --");
        Console.WriteLine(fileContent);

        // ── Step 5: Write to disk ─────────────────────────────────
        string folder   = "LabMigrations";
        Directory.CreateDirectory(folder);

        string fullPath = Path.Combine(folder, fileName);
        File.WriteAllText(fullPath, fileContent);

        Console.WriteLine($"-- Step 5: Written to disk --");
        Console.WriteLine($"Path: {fullPath}");
        Console.WriteLine();

        // ── Verify it's there ─────────────────────────────────────
        Console.WriteLine("-- Verify: files in LabMigrations/ --");
        foreach (var f in Directory.GetFiles(folder, "*.sql").OrderBy(f => f))
            Console.WriteLine($"  {Path.GetFileName(f)}");

        Console.WriteLine();

        // ── Key point: no database touched ────────────────────────
        Console.WriteLine("-- Key point --");
        Console.WriteLine("'migrations add' only writes a file.");
        Console.WriteLine("The database is NOT touched until 'migrations apply'.");
        Console.WriteLine("You can review and edit the file before applying.");
        Console.WriteLine();
    }
}
