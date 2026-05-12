// ================================================================
//  08 — Full MigrationRunner from Scratch
//
//  This is a complete, working MigrationRunner that puts every
//  concept from 01-07 together in one class — exactly mirroring
//  MiniOrm's MigrationRunner.cs.
//
//  Read this AFTER understanding 01-07. Every method here
//  is explained by one of the earlier concept files.
//
//  REQUIRES: Postgres running + MINIORM_CONN env var set.
// ================================================================

using Npgsql;

namespace MigrationRunnerLab.Concepts;

public static class Concept08_FullMigrationRunner
{
    public static async Task RunAsync(string connStr)
    {
        Console.WriteLine("=== 08: Full MigrationRunner End-to-End ===\n");

        var runner = new LabMigrationRunner(connStr);

        // ── Demonstrate the full lifecycle ────────────────────────

        // 1. add — generate migration file
        Console.WriteLine(">>> migrations add CreateProductsAndOrders");
        runner.Add("CreateProductsAndOrders");
        Console.WriteLine();

        // 2. list — show pending
        Console.WriteLine(">>> migrations list");
        await runner.ListAsync();
        Console.WriteLine();

        // 3. apply — run the migration
        Console.WriteLine(">>> migrations apply");
        await runner.ApplyAsync();
        Console.WriteLine();

        // 4. list again — now shows applied
        Console.WriteLine(">>> migrations list (after apply)");
        await runner.ListAsync();
        Console.WriteLine();

        // 5. apply again — nothing to do
        Console.WriteLine(">>> migrations apply (again)");
        await runner.ApplyAsync();
        Console.WriteLine();

        // 6. rollback — undo the last migration
        Console.WriteLine(">>> migrations rollback");
        await runner.RollbackAsync();
        Console.WriteLine();

        // 7. list — back to pending
        Console.WriteLine(">>> migrations list (after rollback)");
        await runner.ListAsync();
        Console.WriteLine();
    }
}

// ================================================================
//  LabMigrationRunner — identical structure to MiniOrm's
//  MigrationRunner, with extra Console.WriteLine comments
//  so you can follow exactly what is happening.
// ================================================================
public sealed class LabMigrationRunner(string connStr)
{
    private const string MigrationsTable  = "__migrations";
    private const string MigrationsFolder = "LabMigrations";

    // ── add ──────────────────────────────────────────────────────
    public void Add(string name)
    {
        Directory.CreateDirectory(MigrationsFolder);

        // Timestamp ensures files sort chronologically
        string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        string fileName  = $"{timestamp}_{name}.sql";
        string path      = Path.Combine(MigrationsFolder, fileName);

        // Build up/down SQL for our two entities
        string upSql =
            "CREATE TABLE IF NOT EXISTS lab_products (\n" +
            "    id       SERIAL PRIMARY KEY,\n" +
            "    name     TEXT NOT NULL,\n" +
            "    price    NUMERIC NOT NULL,\n" +
            "    discount NUMERIC NULL,\n" +
            "    in_stock BOOLEAN NOT NULL\n" +
            ");\n\n" +
            "CREATE TABLE IF NOT EXISTS lab_orders (\n" +
            "    id         SERIAL PRIMARY KEY,\n" +
            "    product_id INTEGER NOT NULL,\n" +
            "    quantity   INTEGER NOT NULL,\n" +
            "    note       TEXT NULL,\n" +
            "    placed_at  TIMESTAMP NOT NULL\n" +
            ");";

        string downSql =
            "DROP TABLE IF EXISTS lab_orders;\n" +
            "DROP TABLE IF EXISTS lab_products;";

        string content =
            "-- up\n"       + upSql   +
            "\n\n-- down\n" + downSql + "\n";

        File.WriteAllText(path, content);
        Console.WriteLine($"Created: {fileName}");
    }

    // ── apply ─────────────────────────────────────────────────────
    public async Task ApplyAsync()
    {
        // 1. Make sure __migrations table exists
        await EnsureMigrationsTableAsync();

        // 2. Load the set of already-applied filenames
        var applied = await GetAppliedMigrationsAsync();

        // 3. Find files not yet applied, sorted chronologically
        var pending = GetMigrationFiles()
            .Where(f => !applied.Contains(Path.GetFileName(f)))
            .OrderBy(f => f)
            .ToList();

        if (pending.Count == 0)
        {
            Console.WriteLine("No pending migrations.");
            return;
        }

        // 4. Open ONE connection and apply all pending files
        using var conn = new NpgsqlConnection(connStr);
        await conn.OpenAsync();

        foreach (var file in pending)
        {
            // Extract just the -- up section
            string sql = ExtractSection(await File.ReadAllTextAsync(file), "up");

            // Run it against Postgres
            using var cmd = new NpgsqlCommand(sql, conn);
            await cmd.ExecuteNonQueryAsync();

            // Record it in __migrations
            await RecordMigrationAsync(conn, Path.GetFileName(file));
            Console.WriteLine($"Applied: {Path.GetFileName(file)}");
        }
    }

    // ── list ──────────────────────────────────────────────────────
    public async Task ListAsync()
    {
        await EnsureMigrationsTableAsync();

        var applied = await GetAppliedMigrationsAsync();
        var files   = GetMigrationFiles().OrderBy(f => f).ToList();

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
    }

    // ── rollback ──────────────────────────────────────────────────
    public async Task RollbackAsync()
    {
        await EnsureMigrationsTableAsync();

        // Find the most recently applied migration
        string? last = await GetLastAppliedMigrationAsync();
        if (last == null) { Console.WriteLine("Nothing to rollback."); return; }

        string path = Path.Combine(MigrationsFolder, last);
        if (!File.Exists(path))
            throw new FileNotFoundException($"Migration file not found: {path}");

        // Extract and run the -- down section
        string sql = ExtractSection(await File.ReadAllTextAsync(path), "down");

        using var conn = new NpgsqlConnection(connStr);
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(sql, conn);
        await cmd.ExecuteNonQueryAsync();

        // Remove from __migrations table
        await DeleteMigrationRecordAsync(conn, last);
        Console.WriteLine($"Rolled back: {last}");
    }

    // ── Private helpers ───────────────────────────────────────────

    private async Task EnsureMigrationsTableAsync()
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

    private async Task<HashSet<string>> GetAppliedMigrationsAsync()
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

    private async Task<string?> GetLastAppliedMigrationAsync()
    {
        using var conn = new NpgsqlConnection(connStr);
        await conn.OpenAsync();
        using var cmd = new NpgsqlCommand(
            $"SELECT file_name FROM {MigrationsTable} ORDER BY id DESC LIMIT 1", conn);
        return await cmd.ExecuteScalarAsync() as string;
    }

    private static async Task RecordMigrationAsync(NpgsqlConnection conn, string fileName)
    {
        using var cmd = new NpgsqlCommand(
            $"INSERT INTO {MigrationsTable} (file_name) VALUES (@fn)", conn);
        cmd.Parameters.AddWithValue("fn", fileName);
        await cmd.ExecuteNonQueryAsync();
    }

    private static async Task DeleteMigrationRecordAsync(NpgsqlConnection conn, string fileName)
    {
        using var cmd = new NpgsqlCommand(
            $"DELETE FROM {MigrationsTable} WHERE file_name = @fn", conn);
        cmd.Parameters.AddWithValue("fn", fileName);
        await cmd.ExecuteNonQueryAsync();
    }

    private static IEnumerable<string> GetMigrationFiles() =>
        Directory.Exists(MigrationsFolder)
            ? Directory.GetFiles(MigrationsFolder, "*.sql")
            : [];

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
