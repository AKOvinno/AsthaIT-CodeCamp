// ================================================================
//  02 — The __migrations Table
//
//  MigrationRunner tracks which migrations have been applied
//  by storing their filenames in a __migrations table.
//  This is how it knows what is "pending" vs "applied".
//
//  REQUIRES: Postgres running + MINIORM_CONN env var set.
// ================================================================

using Npgsql;

namespace MigrationRunnerLab.Concepts;

public static class Concept02_MigrationsTable
{
    public static async Task RunAsync(string connStr)
    {
        Console.WriteLine("=== 02: The __migrations Table ===\n");

        using var conn = new NpgsqlConnection(connStr);
        await conn.OpenAsync();

        // ── Creating the __migrations table ───────────────────────
        // This is exactly what EnsureMigrationsTableAsync() does.
        // "IF NOT EXISTS" makes it safe to call multiple times.

        Console.WriteLine("-- Creating __migrations table --");
        using var createCmd = new NpgsqlCommand("""
            CREATE TABLE IF NOT EXISTS __migrations (
                id         SERIAL PRIMARY KEY,
                file_name  TEXT NOT NULL UNIQUE,
                applied_at TIMESTAMP NOT NULL DEFAULT NOW()
            );
            """, conn);
        await createCmd.ExecuteNonQueryAsync();
        Console.WriteLine("Table created (or already existed).");
        Console.WriteLine();

        // ── Recording a migration as applied ──────────────────────
        // This is what RecordMigrationAsync() does after running -- up

        Console.WriteLine("-- Recording migrations --");
        await RecordMigration(conn, "20240101000000_InitialCreate.sql");
        await RecordMigration(conn, "20240102000000_AddDiscount.sql");
        Console.WriteLine("Two migrations recorded.");
        Console.WriteLine();

        // ── Reading applied migrations ─────────────────────────────
        // This is what GetAppliedMigrationsAsync() does
        // to figure out which files are already applied.

        Console.WriteLine("-- Reading applied migrations --");
        var applied = new HashSet<string>();

        using var selectCmd = new NpgsqlCommand(
            "SELECT file_name FROM __migrations", conn);
        using var reader = await selectCmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
            applied.Add(reader.GetString(0));

        foreach (var name in applied)
            Console.WriteLine($"  [applied] {name}");
        Console.WriteLine();

        // ── Getting the last applied migration ────────────────────
        // This is what GetLastAppliedMigrationAsync() does
        // to know which migration to rollback.

        Console.WriteLine("-- Getting last applied migration --");
        using var conn2 = new NpgsqlConnection(connStr);
        await conn2.OpenAsync();
        using var lastCmd = new NpgsqlCommand(
            "SELECT file_name FROM __migrations ORDER BY id DESC LIMIT 1", conn2);

        var last = await lastCmd.ExecuteScalarAsync() as string;
        Console.WriteLine($"Last applied: {last}");
        Console.WriteLine();

        // ── Deleting a migration record (rollback) ────────────────
        // This is what DeleteMigrationRecordAsync() does
        // after running the -- down section.

        Console.WriteLine("-- Deleting last migration record (rollback) --");
        using var conn3 = new NpgsqlConnection(connStr);
        await conn3.OpenAsync();
        using var delCmd = new NpgsqlCommand(
            "DELETE FROM __migrations WHERE file_name = @fn", conn3);
        delCmd.Parameters.AddWithValue("fn", last!);
        await delCmd.ExecuteNonQueryAsync();
        Console.WriteLine($"Deleted record: {last}");
        Console.WriteLine();

        // Clean up
        using var conn4 = new NpgsqlConnection(connStr);
        await conn4.OpenAsync();
        using var cleanup = new NpgsqlCommand(
            "DELETE FROM __migrations", conn4);
        await cleanup.ExecuteNonQueryAsync();
    }

    private static async Task RecordMigration(NpgsqlConnection conn, string fileName)
    {
        using var cmd = new NpgsqlCommand(
            "INSERT INTO __migrations (file_name) VALUES (@fn)", conn);
        cmd.Parameters.AddWithValue("fn", fileName);
        await cmd.ExecuteNonQueryAsync();
        Console.WriteLine($"  Recorded: {fileName}");
    }
}
