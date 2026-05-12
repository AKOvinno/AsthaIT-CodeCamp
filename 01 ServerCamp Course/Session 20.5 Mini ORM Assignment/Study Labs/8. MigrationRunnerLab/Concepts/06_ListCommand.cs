// ================================================================
//  06 — The "migrations list" Command
//
//  List compares .sql files on disk against the __migrations
//  table and prints [applied] or [pending] for each one.
//
//  REQUIRES: Postgres running + MINIORM_CONN env var set.
// ================================================================

using Npgsql;

namespace MigrationRunnerLab.Concepts;

public static class Concept06_ListCommand
{
    private const string MigrationsFolder = "LabMigrations";
    private const string MigrationsTable  = "__migrations";

    public static async Task RunAsync(string connStr)
    {
        Console.WriteLine("=== 06: The 'migrations list' Command ===\n");

        // ── Step 1: Get applied set from __migrations ─────────────
        Console.WriteLine("-- Step 1: Read __migrations table --");
        var applied = await GetAppliedAsync(connStr);
        Console.WriteLine($"Applied in DB: {applied.Count} migration(s)");
        Console.WriteLine();

        // ── Step 2: Get all .sql files from disk ──────────────────
        Console.WriteLine("-- Step 2: Read files from disk --");
        if (!Directory.Exists(MigrationsFolder))
        {
            Console.WriteLine("LabMigrations/ folder not found. Run Concept03 first.");
            return;
        }

        var files = Directory.GetFiles(MigrationsFolder, "*.sql")
            .OrderBy(f => f)
            .ToList();

        Console.WriteLine($"Files on disk: {files.Count}");
        Console.WriteLine();

        // ── Step 3: Compare and print status ─────────────────────
        // For each file: is its filename in the applied HashSet?
        // Yes → [applied]    No → [pending]

        Console.WriteLine("-- Step 3: List with status --");
        if (files.Count == 0)
        {
            Console.WriteLine("No migration files found.");
            return;
        }

        foreach (var file in files)
        {
            string name   = Path.GetFileName(file);
            string status = applied.Contains(name) ? "[applied]" : "[pending]";
            Console.WriteLine($"  {status} {name}");
        }

        Console.WriteLine();

        // ── Why HashSet? ──────────────────────────────────────────
        Console.WriteLine("-- Why HashSet for applied migrations? --");
        Console.WriteLine("HashSet.Contains() is O(1) — instant lookup.");
        Console.WriteLine("If you had 500 migrations, List.Contains() would");
        Console.WriteLine("scan all 500 for every file. HashSet checks instantly.");
        Console.WriteLine();
    }

    private static async Task<HashSet<string>> GetAppliedAsync(string connStr)
    {
        var result = new HashSet<string>();
        try
        {
            using var conn = new NpgsqlConnection(connStr);
            await conn.OpenAsync();
            using var cmd    = new NpgsqlCommand($"SELECT file_name FROM {MigrationsTable}", conn);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                result.Add(reader.GetString(0));
        }
        catch { }
        return result;
    }
}
