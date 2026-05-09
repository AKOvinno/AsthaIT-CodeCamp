// ================================================================
//  03 — Writing Files
//
//  MigrationRunner writes .sql files when you run "migrations add".
//  This covers File.WriteAllText and how to build file content.
// ================================================================

namespace FileAndEnvLab.Concepts;

public static class Concept03_WritingFiles
{
    public static void Run()
    {
        Console.WriteLine("=== 03: Writing Files ===\n");

        // ── WriteAllText — creates or overwrites a file ───────────
        string path = "SampleFiles/written.txt";
        Directory.CreateDirectory("SampleFiles");

        File.WriteAllText(path, "Hello from WriteAllText!");
        Console.WriteLine($"Written to: {path}");
        Console.WriteLine($"Content: {File.ReadAllText(path)}");

        Console.WriteLine();

        // ── Writing a migration file like MigrationRunner.Add() ───
        // This is exactly what MigrationRunner.Add() does:

        Console.WriteLine("-- Simulating MigrationRunner.Add() --");

        string migrationsFolder = "SampleFiles/Migrations";
        Directory.CreateDirectory(migrationsFolder);

        string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        string name      = "InitialCreate";
        string fileName  = $"{timestamp}_{name}.sql";
        string fullPath  = Path.Combine(migrationsFolder, fileName);

        // Build the up/down content
        string upSql   = "CREATE TABLE IF NOT EXISTS products (\n" +
                         "    id       SERIAL PRIMARY KEY,\n" +
                         "    name     TEXT NOT NULL,\n" +
                         "    price    NUMERIC NOT NULL,\n" +
                         "    discount NUMERIC NULL\n" +
                         ");";

        string downSql = "DROP TABLE IF EXISTS products;";

        string fileContent = "-- up\n"   + upSql   +
                             "\n\n-- down\n" + downSql + "\n";

        File.WriteAllText(fullPath, fileContent);

        Console.WriteLine($"Created: {fileName}");
        Console.WriteLine();
        Console.WriteLine("-- File content --");
        Console.WriteLine(File.ReadAllText(fullPath));

        // ── AppendAllText — adds to existing file ─────────────────
        string logPath = "SampleFiles/log.txt";
        File.AppendAllText(logPath, $"[{DateTime.Now}] Migration applied\n");
        File.AppendAllText(logPath, $"[{DateTime.Now}] Another entry\n");
        Console.WriteLine("-- Log file --");
        Console.WriteLine(File.ReadAllText(logPath));
    }
}
