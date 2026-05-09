// ================================================================
//  02 — Reading Files
//
//  MigrationRunner reads .sql files from the Migrations/ folder.
//  This file covers File.ReadAllText, File.ReadAllTextAsync,
//  and how to read files safely.
// ================================================================

namespace FileAndEnvLab.Concepts;

public static class Concept02_ReadingFiles
{
    public static void Run()
    {
        Console.WriteLine("=== 02: Reading Files ===\n");

        // ── Create a sample file to read ──────────────────────────
        string filePath = "SampleFiles/sample_migration.sql";
        Directory.CreateDirectory("SampleFiles");

        File.WriteAllText(filePath,
            "-- up\n" +
            "CREATE TABLE IF NOT EXISTS products (\n" +
            "    id    SERIAL PRIMARY KEY,\n" +
            "    name  TEXT NOT NULL\n" +
            ");\n\n" +
            "-- down\n" +
            "DROP TABLE IF EXISTS products;\n");

        Console.WriteLine($"Created: {filePath}");
        Console.WriteLine();

        // ── ReadAllText — reads the entire file as one string ─────
        string content = File.ReadAllText(filePath);
        Console.WriteLine("-- File content --");
        Console.WriteLine(content);

        // ── ReadAllTextAsync — async version ──────────────────────
        // MigrationRunner uses this:
        // var sql = await File.ReadAllTextAsync(file);
        // We can't use await here outside async — shown in Program.cs

        // ── ReadAllLines — reads as an array of lines ─────────────
        string[] lines = File.ReadAllLines(filePath);
        Console.WriteLine($"-- File has {lines.Length} lines --");
        for (int i = 0; i < lines.Length; i++)
            Console.WriteLine($"  Line {i + 1}: {lines[i]}");

        Console.WriteLine();

        // ── Checking if a file exists before reading ──────────────
        Console.WriteLine("-- File.Exists() --");
        Console.WriteLine($"sample_migration.sql exists: {File.Exists(filePath)}");
        Console.WriteLine($"missing.sql exists:          {File.Exists("missing.sql")}");

        // Always check existence when the file might not be there
        // MigrationRunner checks this before reading the rollback file:
        // if (!File.Exists(path))
        //     throw new FileNotFoundException($"Migration file not found: {path}");

        Console.WriteLine();
    }
}
