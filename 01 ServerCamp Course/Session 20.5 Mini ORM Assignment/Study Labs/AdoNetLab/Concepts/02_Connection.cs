// ================================================================
//  02 — NpgsqlConnection
//
//  A connection represents an open channel to the database.
//  You must open it before sending any commands.
//  You must close/dispose it when done — using var handles this.
//
//  REQUIRES: Postgres running + MINIORM_CONN env var set.
// ================================================================

using Npgsql;

namespace AdoNetLab.Concepts;

public static class Concept02_Connection
{
    public static async Task RunAsync(string connStr)
    {
        Console.WriteLine("=== 02: NpgsqlConnection ===\n");

        // ── Creating a connection ─────────────────────────────────
        // NpgsqlConnection takes a connection string.
        // Just creating it does NOT connect to Postgres yet.
        var conn = new NpgsqlConnection(connStr);
        Console.WriteLine($"State before open: {conn.State}");  // Closed

        // ── Opening the connection ────────────────────────────────
        // OpenAsync() actually connects to Postgres.
        // This is where the network call happens.
        await conn.OpenAsync();
        Console.WriteLine($"State after open:  {conn.State}");  // Open

        // ── Closing and disposing ─────────────────────────────────
        // Always close the connection when done.
        // Dispose() calls Close() automatically.
        await conn.CloseAsync();
        Console.WriteLine($"State after close: {conn.State}");  // Closed
        await conn.DisposeAsync();

        Console.WriteLine();

        // ── The RIGHT way: using var ──────────────────────────────
        // "using var" automatically calls Dispose() when the
        // block ends — even if an exception is thrown.
        // This is the pattern used throughout MiniOrm.

        Console.WriteLine("-- using var pattern (correct way) --");

        using var conn2 = new NpgsqlConnection(connStr);
        await conn2.OpenAsync();
        Console.WriteLine($"Connection open: {conn2.State}");

        // conn2.Dispose() is called automatically here
        // when the method ends or if an exception occurs

        Console.WriteLine("Connection disposed automatically by 'using'");
        Console.WriteLine();

        // ── Why a new connection per operation? ───────────────────
        // MiniOrm opens a new connection inside each method:
        //   public async Task<int> InsertAsync(T entity) {
        //       using var conn = new NpgsqlConnection(_connStr);
        //       await conn.OpenAsync();
        //       ...
        //   }
        //
        // This is called "connection per operation".
        // Npgsql has connection pooling — reusing connections
        // internally is handled automatically. You don't need to
        // worry about performance here.

        Console.WriteLine("-- Connection per operation (MiniOrm style) --");
        await SimulateInsertAsync(connStr);
        await SimulateInsertAsync(connStr);
        Console.WriteLine("Both operations got their own connection ✓");
        Console.WriteLine();
    }

    private static async Task SimulateInsertAsync(string connStr)
    {
        // Each operation opens and closes its own connection
        using var conn = new NpgsqlConnection(connStr);
        await conn.OpenAsync();
        Console.WriteLine($"  Opened connection (State: {conn.State})");
        // conn disposed automatically here
    }
}
