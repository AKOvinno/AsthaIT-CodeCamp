// ================================================================
//  07 — The "migrations rollback" Command
//
//  Rollback undoes the LAST applied migration.
//  It reads the -- down section of that file and runs it,
//  then deletes the record from __migrations.
//
//  REQUIRES: Postgres running + MINIORM_CONN env var set.
// ================================================================

using Npgsql;

namespace MigrationRunnerLab.Concepts;

public static class Concept07_RollbackCommand
{
    private const string MigrationsFolder = "LabMigrations";
    private const string MigrationsTable  = "__migrations";

    public static async Task RunAsync(string connStr)
    {
        Console.WriteLine("=== 07: The 'migrations rollback' Command ===\n");

        // ── Step 1: Find the last applied migration ────────────────
        Console.WriteLine("-- Step 1: GetLastAppliedMigration --");
        string? last = await GetLastAppliedAsync(connStr);

        if (last == null)
        {
            Console.WriteLine("Nothing to rollback — __migrations table is empty.");
            return;
        }

        Console.WriteLine($"Last applied: {last}");
        Console.WriteLine();

        // ── Step 2: Find the file on disk ─────────────────────────
        Console.WriteLine("-- Step 2: Locate file on disk --");
        string path = Path.Combine(MigrationsFolder, last);

        if (!File.Exists(path))
        {
            Console.WriteLine($"ERROR: File not found: {path}");
            Console.WriteLine("Cannot rollback — the .sql file has been deleted.");
            return;
        }

        Console.WriteLine($"Found: {path}");
        Console.WriteLine();

        // ── Step 3: Extract the -- down SQL ───────────────────────
        Console.WriteLine("-- Step 3: Extract -- down section --");
        string content = await File.ReadAllTextAsync(path);
        string downSql = ExtractSection(content, "down");

        Console.WriteLine("Down SQL to run:");
        Console.WriteLine(downSql);
        Console.WriteLine();

        // ── Step 4: Run -- down against Postgres ──────────────────
        Console.WriteLine("-- Step 4: Execute -- down SQL --");
        using var conn = new NpgsqlConnection(connStr);
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(downSql, conn);
        await cmd.ExecuteNonQueryAsync();
        Console.WriteLine("Down SQL executed successfully.");
        Console.WriteLine();

        // ── Step 5: Remove from __migrations ─────────────────────
        Console.WriteLine("-- Step 5: Remove from __migrations --");
        using var delCmd = new NpgsqlCommand(
            $"DELETE FROM {MigrationsTable} WHERE file_name = @fn", conn);
        delCmd.Parameters.AddWithValue("fn", last);
        await delCmd.ExecuteNonQueryAsync();
        Console.WriteLine($"Removed record: {last}");
        Console.WriteLine();

        Console.WriteLine($"Rollback complete. '{last}' has been reverted ✓");
        Console.WriteLine();

        // ── Key point: only ONE migration at a time ────────────────
        Console.WriteLine("-- Key point --");
        Console.WriteLine("Rollback only reverts the LAST applied migration.");
        Console.WriteLine("To rollback multiple, run 'rollback' multiple times.");
        Console.WriteLine("This gives you fine-grained control over what you undo.");
        Console.WriteLine();
    }

    private static async Task<string?> GetLastAppliedAsync(string connStr)
    {
        using var conn = new NpgsqlConnection(connStr);
        await conn.OpenAsync();
        using var cmd = new NpgsqlCommand(
            $"SELECT file_name FROM {MigrationsTable} ORDER BY id DESC LIMIT 1", conn);
        return await cmd.ExecuteScalarAsync() as string;
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
