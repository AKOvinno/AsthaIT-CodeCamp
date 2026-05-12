// ================================================================
//  05 — NpgsqlDataReader
//
//  DataReader reads SELECT results row by row.
//  It is a "forward-only cursor" — you can only move forward,
//  never backward. Read() returns true while there are more rows.
//
//  MiniOrm's DbSet.Map() uses reader[columnName] to fill entities.
//
//  REQUIRES: Postgres running + MINIORM_CONN env var set.
//            Run 00_Setup.sql in Postgres first.
// ================================================================

using Npgsql;

namespace AdoNetLab.Concepts;

public static class Concept05_DataReader
{
    public static async Task RunAsync(string connStr)
    {
        Console.WriteLine("=== 05: NpgsqlDataReader ===\n");

        using var conn = new NpgsqlConnection(connStr);
        await conn.OpenAsync();

        // Seed some data
        await SeedData(conn);

        // ── Basic reading ─────────────────────────────────────────
        Console.WriteLine("-- Reading rows with reader --");

        using var cmd    = new NpgsqlCommand("SELECT * FROM adolab_products", conn);
        using var reader = await cmd.ExecuteReaderAsync();

        // reader.ReadAsync() moves to next row
        // Returns true  → a row is available, read it
        // Returns false → no more rows, stop
        while (await reader.ReadAsync())
        {
            // Access by column NAME (safer — column order doesn't matter)
            object rawDiscount = reader["discount"];
            decimal? discount  = rawDiscount == DBNull.Value ? null : (decimal)rawDiscount;

            Console.WriteLine(
                $"  id={reader["id"]}, " +
                $"name={reader["name"]}, " +
                $"price={reader["price"]}, " +
                $"discount={discount?.ToString() ?? "NULL"}");
        }

        Console.WriteLine();

        // ── Reading a single row (FindById pattern) ───────────────
        Console.WriteLine("-- FindById pattern --");

        using var conn2   = new NpgsqlConnection(connStr);
        await conn2.OpenAsync();
        using var findCmd = new NpgsqlCommand(
            "SELECT * FROM adolab_products WHERE id = @id", conn2);
        findCmd.Parameters.AddWithValue("id", 1);

        using var findReader = await findCmd.ExecuteReaderAsync();

        // ReadAsync() once — either we have a row or we don't
        if (await findReader.ReadAsync())
        {
            Console.WriteLine($"Found: id={findReader["id"]}, name={findReader["name"]}");
        }
        else
        {
            Console.WriteLine("Not found — ReadAsync() returned false");
        }

        Console.WriteLine();

        // ── GetInt32, GetString etc. vs reader[name] ──────────────
        Console.WriteLine("-- Two ways to read column values --");

        using var conn3   = new NpgsqlConnection(connStr);
        await conn3.OpenAsync();
        using var cmd3    = new NpgsqlCommand("SELECT * FROM adolab_products LIMIT 1", conn3);
        using var reader3 = await cmd3.ExecuteReaderAsync();

        if (await reader3.ReadAsync())
        {
            // Way 1: By column name — returns object, you handle casting
            object nameObj = reader3["name"];
            Console.WriteLine($"reader[\"name\"] = {nameObj} (type: {nameObj.GetType().Name})");

            // Way 2: Typed helper methods — GetString, GetInt32, GetDecimal
            // These are type-safe but throw if the value is DBNull
            string typedName = reader3.GetString(reader3.GetOrdinal("name"));
            Console.WriteLine($"GetString(\"name\") = {typedName}");

            // MiniOrm uses reader[columnName] because it handles
            // DBNull.Value gracefully without throwing
        }

        Console.WriteLine();
    }

    private static async Task SeedData(NpgsqlConnection conn)
    {
        // Clear and re-seed for clean test
        using var del = new NpgsqlCommand("DELETE FROM adolab_products", conn);
        await del.ExecuteNonQueryAsync();

        var products = new[]
        {
            ("Keyboard", 89.99m, (decimal?)null),
            ("Mouse",    29.99m, (decimal?)2.50m),
            ("Monitor",  299.99m,(decimal?)null),
        };

        foreach (var (name, price, discount) in products)
        {
            using var ins = new NpgsqlCommand(
                "INSERT INTO adolab_products (name, price, discount) VALUES (@p0, @p1, @p2)",
                conn);
            ins.Parameters.AddWithValue("p0", name);
            ins.Parameters.AddWithValue("p1", price);
            ins.Parameters.AddWithValue("p2", (object?)discount ?? DBNull.Value);
            await ins.ExecuteNonQueryAsync();
        }
    }
}
