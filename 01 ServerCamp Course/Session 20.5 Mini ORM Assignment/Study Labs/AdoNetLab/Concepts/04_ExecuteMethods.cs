// ================================================================
//  04 — The Three Execute Methods
//
//  ExecuteNonQueryAsync  → INSERT, UPDATE, DELETE
//  ExecuteScalarAsync    → Single value (RETURNING id)
//  ExecuteReaderAsync    → SELECT rows → NpgsqlDataReader
//
//  REQUIRES: Postgres running + MINIORM_CONN env var set.
//            Run 00_Setup.sql in Postgres first.
// ================================================================

using Npgsql;

namespace AdoNetLab.Concepts;

public static class Concept04_ExecuteMethods
{
    public static async Task RunAsync(string connStr)
    {
        Console.WriteLine("=== 04: The Three Execute Methods ===\n");

        using var conn = new NpgsqlConnection(connStr);
        await conn.OpenAsync();

        // ── ExecuteScalarAsync ────────────────────────────────────
        // Returns ONE value — the first column of the first row.
        // MiniOrm uses this for INSERT ... RETURNING id

        Console.WriteLine("-- ExecuteScalarAsync --");
        using var insertCmd = new NpgsqlCommand(
            "INSERT INTO adolab_products (name, price, discount) VALUES (@p0, @p1, @p2) RETURNING id",
            conn);
        insertCmd.Parameters.AddWithValue("p0", "Keyboard");
        insertCmd.Parameters.AddWithValue("p1", 89.99m);
        insertCmd.Parameters.AddWithValue("p2", DBNull.Value);

        // ExecuteScalarAsync returns object? — we cast to int
        var id = (int)(await insertCmd.ExecuteScalarAsync())!;
        Console.WriteLine($"Inserted Keyboard with id = {id}");

        Console.WriteLine();

        // ── ExecuteNonQueryAsync ──────────────────────────────────
        // For UPDATE and DELETE — returns number of rows affected.
        // MiniOrm uses this in UpdateAsync and DeleteAsync.

        Console.WriteLine("-- ExecuteNonQueryAsync (UPDATE) --");
        using var updateCmd = new NpgsqlCommand(
            "UPDATE adolab_products SET price = @p0 WHERE id = @pkVal",
            conn);
        updateCmd.Parameters.AddWithValue("p0",    79.99m);
        updateCmd.Parameters.AddWithValue("pkVal", id);

        int rowsAffected = await updateCmd.ExecuteNonQueryAsync();
        Console.WriteLine($"Updated {rowsAffected} row(s)");

        Console.WriteLine();

        Console.WriteLine("-- ExecuteNonQueryAsync (DELETE) --");
        using var deleteCmd = new NpgsqlCommand(
            "DELETE FROM adolab_products WHERE id = @id",
            conn);
        deleteCmd.Parameters.AddWithValue("id", id);

        rowsAffected = await deleteCmd.ExecuteNonQueryAsync();
        Console.WriteLine($"Deleted {rowsAffected} row(s)");

        Console.WriteLine();

        // ── ExecuteReaderAsync ────────────────────────────────────
        // Returns a NpgsqlDataReader for SELECT queries.
        // MiniOrm uses this in FindByIdAsync and GetAllAsync.

        Console.WriteLine("-- ExecuteReaderAsync (SELECT) --");

        // Insert a couple of rows first
        await InsertProduct(conn, "Monitor", 299.99m, null);
        await InsertProduct(conn, "Desk",    149.99m, 10.00m);

        using var selectCmd = new NpgsqlCommand(
            "SELECT id, name, price, discount FROM adolab_products",
            conn);

        using var reader = await selectCmd.ExecuteReaderAsync();

        // Read() moves to the next row — returns false when done
        Console.WriteLine("Rows from SELECT:");
        while (await reader.ReadAsync())
        {
            int    rowId    = reader.GetInt32(0);              // column index
            string name     = reader.GetString(1);
            decimal price   = reader.GetDecimal(2);
            object discount = reader["discount"];               // by column name

            string discountStr = discount == DBNull.Value ? "NULL" : discount.ToString()!;
            Console.WriteLine($"  id={rowId}, name={name}, price={price}, discount={discountStr}");
        }

        Console.WriteLine();
    }

    private static async Task InsertProduct(NpgsqlConnection conn, string name, decimal price, decimal? discount)
    {
        using var cmd = new NpgsqlCommand(
            "INSERT INTO adolab_products (name, price, discount) VALUES (@p0, @p1, @p2)",
            conn);
        cmd.Parameters.AddWithValue("p0", name);
        cmd.Parameters.AddWithValue("p1", price);
        cmd.Parameters.AddWithValue("p2", (object?)discount ?? DBNull.Value);
        await cmd.ExecuteNonQueryAsync();
    }
}
