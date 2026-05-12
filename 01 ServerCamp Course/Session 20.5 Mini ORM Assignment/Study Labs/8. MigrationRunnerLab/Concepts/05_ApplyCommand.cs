// ================================================================
//  05 — The "migrations apply" Command
//
//  Apply finds every .sql file that is NOT yet in __migrations,
//  runs their -- up SQL against Postgres in order,
//  and records each one in __migrations.
//
//  REQUIRES: Postgres running + MINIORM_CONN env var set.
// ================================================================

using Npgsql;

namespace MigrationRunnerLab.Concepts;

public static class Concept05_ApplyCommand
{
    private const string MigrationsFolder = "LabMigrations";
    private const string MigrationsTable  = "__migrations";

    public static async Task RunAsync(string connStr)
    {
        Console.WriteLine("=== 05: The 'migrations apply' Command ===\n");

        // ── Step 1: Ensure __migrations table exists ───────────────
        Console.WriteLine("-- Step 1: EnsureMigrationsTable --");
        await EnsureMigrationsTableAsync(connStr);
        Console.WriteLine("__migrations table ready.");
        Console.WriteLine();

        // ── Step 2: Get already-applied migration filenames ────────
        Console.WriteLine("-- Step 2: GetAppliedMigrations --");
        var applied = await GetAppliedMigrationsAsync(connStr);
        Console.WriteLine($"Currently applied: {applied.Count} migration(s)");
        foreach (var a in applied)
            Console.WriteLine($"  [applied] {a}");
        Console.WriteLine();

        // ── Step 3: Get all .sql files on disk ────────────────────
        Console.WriteLine("-- Step 3: Get all .sql files --");
        if (!Directory.Exists(MigrationsFolder))
        {
            Console.WriteLine("No LabMigrations/ folder found.");
            Console.WriteLine("Run Concept03_AddCommand.Run() first to generate a migration.");
            return;
        }

        var allFiles = Directory.GetFiles(MigrationsFolder, "*.sql")
            .OrderBy(f => f)      // chronological order by filename
            .ToList();

        Console.WriteLine($"Found {allFiles.Count} file(s) on disk:");
        foreach (var f in allFiles)
            Console.WriteLine($"  {Path.GetFileName(f)}");
        Console.WriteLine();

        // ── Step 4: Find pending (not yet applied) ────────────────
        Console.WriteLine("-- Step 4: Find pending migrations --");
        var pending = allFiles
            .Where(f => !applied.Contains(Path.GetFileName(f)))
            .ToList();

        if (pending.Count == 0)
        {
            Console.WriteLine("No pending migrations. Everything is up to date.");
            return;
        }

        Console.WriteLine($"Pending: {pending.Count} migration(s):");
        foreach (var p in pending)
            Console.WriteLine($"  [pending] {Path.GetFileName(p)}");
        Console.WriteLine();

        // ── Step 5: Run each pending migration's -- up SQL ────────
        Console.WriteLine("-- Step 5: Apply each pending migration --");

        using var conn = new NpgsqlConnection(connStr);
        await conn.OpenAsync();

        foreach (var file in pending)
        {
            string content = await File.ReadAllTextAsync(file);
            string upSql   = ExtractSection(content, "up");

            Console.WriteLine($"Applying: {Path.GetFileName(file)}");
            Console.WriteLine($"SQL:\n{upSql}\n");

            // Run the -- up SQL against Postgres
            using var cmd = new NpgsqlCommand(upSql, conn);
            await cmd.ExecuteNonQueryAsync();

            // Record it as applied
            await RecordMigrationAsync(conn, Path.GetFileName(file));
            Console.WriteLine($"Applied and recorded: {Path.GetFileName(file)} ✓");
            Console.WriteLine();
        }
    }

    // ── Helpers (same as in MigrationRunner) ─────────────────────

    private static async Task EnsureMigrationsTableAsync(string connStr)
    {
        using var conn = new NpgsqlConnection(connStr);
        await conn.OpenAsync();
        using var cmd = new NpgsqlCommand($"""
            CREATE TABLE IF NOT EXISTS {MigrationsTable} (
                id         SERIAL PRIMARY KEY,
                file_name  TEXT NOT NULL UNIQUE,
                applied_at TIMESTAMP NOT NULL DEFAULT NOW()
            );
            """, conn);
        await cmd.ExecuteNonQueryAsync();
    }

    private static async Task<HashSet<string>> GetAppliedMigrationsAsync(string connStr)
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
        catch { /* table may not exist yet on very first run */ }
        return result;
    }

    private static async Task RecordMigrationAsync(NpgsqlConnection conn, string fileName)
    {
        using var cmd = new NpgsqlCommand(
            $"INSERT INTO {MigrationsTable} (file_name) VALUES (@fn)", conn);
        cmd.Parameters.AddWithValue("fn", fileName);
        await cmd.ExecuteNonQueryAsync();
    }

    private static string ExtractSection(string content, string section)
    {
        var lines   = content.Split('\n');
        var capture = false;
        var result  = new List<string>();
        foreach (var line in lines)
        {
            if (line.TrimStart().StartsWith($"-- {section}")) { capture = true;  continue; }
            if (line.TrimStart().StartsWith("-- ") && capture) { break; }
            if (capture) result.Add(line);
        }
        return string.Join('\n', result).Trim();
    }
}
