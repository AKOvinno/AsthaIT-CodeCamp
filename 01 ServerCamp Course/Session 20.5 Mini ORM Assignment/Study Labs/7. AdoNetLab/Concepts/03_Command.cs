// ================================================================
//  03 — NpgsqlCommand and Parameters
//
//  NpgsqlCommand holds the SQL you want to run.
//  Parameters (@p0, @name etc.) let you safely pass values
//  without concatenating them into the SQL string.
//
//  REQUIRES: Postgres running + MINIORM_CONN env var set.
//            Run 00_Setup.sql in Postgres first.
// ================================================================

using Npgsql;

namespace AdoNetLab.Concepts;

public static class Concept03_Command
{
    public static async Task RunAsync(string connStr)
    {
        Console.WriteLine("=== 03: NpgsqlCommand and Parameters ===\n");

        // ── Creating a command ────────────────────────────────────
        using var conn = new NpgsqlConnection(connStr);
        await conn.OpenAsync();

        // NpgsqlCommand takes the SQL string and the connection
        using var cmd = new NpgsqlCommand("SELECT 1 + 1", conn);

        // ExecuteScalarAsync returns the first column of first row
        var result = await cmd.ExecuteScalarAsync();
        Console.WriteLine($"SELECT 1 + 1 = {result}");  // 2

        Console.WriteLine();

        // ── Why parameters? SQL Injection ─────────────────────────
        // NEVER do this — it's vulnerable to SQL injection:
        // string name = userInput;
        // var cmd = new NpgsqlCommand($"SELECT * FROM products WHERE name = '{name}'", conn);
        //
        // If name = "'; DROP TABLE products; --"
        // the query becomes: SELECT * FROM products WHERE name = ''; DROP TABLE products; --'
        // Your table is GONE.

        Console.WriteLine("-- Parameterised queries (safe) --");

        // CORRECT — use @param placeholders
        string searchName = "Keyboard";
        using var safeCmd = new NpgsqlCommand(
            "SELECT * FROM adolab_products WHERE name = @name", conn);

        // AddWithValue adds the parameter value safely
        // Npgsql handles escaping — SQL injection is impossible
        safeCmd.Parameters.AddWithValue("name", searchName);

        Console.WriteLine($"Safe query with @name = '{searchName}'");
        Console.WriteLine("Npgsql escapes the value — SQL injection impossible");

        Console.WriteLine();

        // ── Multiple parameters ───────────────────────────────────
        // MiniOrm uses @p0, @p1, @p2... for INSERT statements
        Console.WriteLine("-- Multiple parameters (@p0, @p1...) --");

        using var insertCmd = new NpgsqlCommand(
            "INSERT INTO adolab_products (name, price, discount) VALUES (@p0, @p1, @p2) RETURNING id",
            conn);

        // Add each parameter in order
        insertCmd.Parameters.AddWithValue("p0", "Mouse");
        insertCmd.Parameters.AddWithValue("p1", 29.99m);
        insertCmd.Parameters.AddWithValue("p2", DBNull.Value);  // null discount

        Console.WriteLine("Parameters added:");
        Console.WriteLine("  @p0 = 'Mouse'");
        Console.WriteLine("  @p1 = 29.99");
        Console.WriteLine("  @p2 = DBNull.Value  (NULL in Postgres)");

        // Execute and get the new id
        var newId = await insertCmd.ExecuteScalarAsync();
        Console.WriteLine($"Inserted with id = {newId}");

        Console.WriteLine();
    }
}
