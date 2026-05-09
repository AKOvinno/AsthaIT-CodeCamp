// ================================================================
//  06 — Full MiniOrm DbSet Walkthrough with ADO.NET
//
//  This traces every single ADO.NET call that happens when
//  you call InsertAsync, FindByIdAsync, GetAllAsync,
//  UpdateAsync, and DeleteAsync on DbSet<Product>.
//
//  REQUIRES: Postgres running + MINIORM_CONN env var set.
//            Run 00_Setup.sql in Postgres first.
// ================================================================

using Npgsql;

namespace AdoNetLab.Concepts;

public static class Concept06_MiniOrmDbSetWalkthrough
{
    public static async Task RunAsync(string connStr)
    {
        Console.WriteLine("=== 06: Full MiniOrm DbSet ADO.NET Walkthrough ===\n");

        // ── InsertAsync ───────────────────────────────────────────
        Console.WriteLine("-- InsertAsync --");
        Console.WriteLine("SQL: INSERT INTO adolab_products (name, price, discount) VALUES (@p0, @p1, @p2) RETURNING id");

        using var insertConn = new NpgsqlConnection(connStr);
        await insertConn.OpenAsync();
        using var insertCmd = new NpgsqlCommand(
            "INSERT INTO adolab_products (name, price, discount) VALUES (@p0, @p1, @p2) RETURNING id",
            insertConn);

        // @p0 = name (string, NOT NULL)
        insertCmd.Parameters.AddWithValue("p0", "Keyboard");
        // @p1 = price (decimal, NOT NULL)
        insertCmd.Parameters.AddWithValue("p1", 89.99m);
        // @p2 = discount (decimal?, NULL allowed) — null → DBNull.Value
        insertCmd.Parameters.AddWithValue("p2", DBNull.Value);

        // ExecuteScalarAsync → returns the RETURNING id value
        var insertedId = (int)(await insertCmd.ExecuteScalarAsync())!;
        Console.WriteLine($"Inserted. New id = {insertedId}");
        Console.WriteLine();

        // ── FindByIdAsync ─────────────────────────────────────────
        Console.WriteLine("-- FindByIdAsync --");
        Console.WriteLine($"SQL: SELECT * FROM adolab_products WHERE id = @id");

        using var findConn = new NpgsqlConnection(connStr);
        await findConn.OpenAsync();
        using var findCmd = new NpgsqlCommand(
            "SELECT * FROM adolab_products WHERE id = @id",
            findConn);
        findCmd.Parameters.AddWithValue("id", insertedId);

        using var findReader = await findCmd.ExecuteReaderAsync();

        if (await findReader.ReadAsync())
        {
            // Map each column back to a C# value
            // DBNull.Value → null for nullable columns
            int     id       = (int)findReader["id"];
            string  name     = (string)findReader["name"];
            decimal price    = (decimal)findReader["price"];
            object  rawDisc  = findReader["discount"];
            decimal? discount = rawDisc == DBNull.Value ? null : (decimal)rawDisc;

            Console.WriteLine($"Found: id={id}, name={name}, price={price}, discount={discount?.ToString() ?? "NULL"}");
        }
        Console.WriteLine();

        // ── GetAllAsync ───────────────────────────────────────────
        Console.WriteLine("-- GetAllAsync --");
        Console.WriteLine("SQL: SELECT * FROM adolab_products");

        // Insert a second product first
        using var ins2Conn = new NpgsqlConnection(connStr);
        await ins2Conn.OpenAsync();
        using var ins2Cmd = new NpgsqlCommand(
            "INSERT INTO adolab_products (name, price, discount) VALUES (@p0, @p1, @p2) RETURNING id",
            ins2Conn);
        ins2Cmd.Parameters.AddWithValue("p0", "Mouse");
        ins2Cmd.Parameters.AddWithValue("p1", 29.99m);
        ins2Cmd.Parameters.AddWithValue("p2", 2.50m);
        var id2 = (int)(await ins2Cmd.ExecuteScalarAsync())!;

        using var allConn = new NpgsqlConnection(connStr);
        await allConn.OpenAsync();
        using var allCmd    = new NpgsqlCommand("SELECT * FROM adolab_products", allConn);
        using var allReader = await allCmd.ExecuteReaderAsync();

        var results = new List<string>();
        while (await allReader.ReadAsync())   // loops until no more rows
        {
            results.Add($"id={allReader["id"]}, name={allReader["name"]}");
        }
        Console.WriteLine($"GetAll returned {results.Count} rows:");
        results.ForEach(r => Console.WriteLine($"  {r}"));
        Console.WriteLine();

        // ── UpdateAsync ───────────────────────────────────────────
        Console.WriteLine("-- UpdateAsync --");
        Console.WriteLine("SQL: UPDATE adolab_products SET name = @p0, price = @p1, discount = @p2 WHERE id = @pkVal");

        using var updConn = new NpgsqlConnection(connStr);
        await updConn.OpenAsync();
        using var updCmd = new NpgsqlCommand(
            "UPDATE adolab_products SET name = @p0, price = @p1, discount = @p2 WHERE id = @pkVal",
            updConn);
        updCmd.Parameters.AddWithValue("p0",    "Keyboard Pro");  // updated name
        updCmd.Parameters.AddWithValue("p1",    79.99m);           // updated price
        updCmd.Parameters.AddWithValue("p2",    5.00m);            // added discount
        updCmd.Parameters.AddWithValue("pkVal", insertedId);        // WHERE id = ?

        int rowsAffected = await updCmd.ExecuteNonQueryAsync();
        Console.WriteLine($"Updated {rowsAffected} row(s)");
        Console.WriteLine();

        // ── DeleteAsync ───────────────────────────────────────────
        Console.WriteLine("-- DeleteAsync --");
        Console.WriteLine("SQL: DELETE FROM adolab_products WHERE id = @id");

        using var delConn = new NpgsqlConnection(connStr);
        await delConn.OpenAsync();
        using var delCmd = new NpgsqlCommand(
            "DELETE FROM adolab_products WHERE id = @id",
            delConn);
        delCmd.Parameters.AddWithValue("id", insertedId);
        await delCmd.ExecuteNonQueryAsync();
        Console.WriteLine($"Deleted id={insertedId}");

        // Clean up second row too
        using var del2Conn = new NpgsqlConnection(connStr);
        await del2Conn.OpenAsync();
        using var del2Cmd = new NpgsqlCommand(
            "DELETE FROM adolab_products WHERE id = @id", del2Conn);
        del2Cmd.Parameters.AddWithValue("id", id2);
        await del2Cmd.ExecuteNonQueryAsync();
        Console.WriteLine($"Deleted id={id2}");

        Console.WriteLine();
        Console.WriteLine("-- Summary: ADO.NET calls in MiniOrm DbSet --");
        Console.WriteLine("  InsertAsync   → ExecuteScalarAsync  (gets RETURNING id)");
        Console.WriteLine("  FindByIdAsync → ExecuteReaderAsync  (reads one row)");
        Console.WriteLine("  GetAllAsync   → ExecuteReaderAsync  (loops all rows)");
        Console.WriteLine("  UpdateAsync   → ExecuteNonQueryAsync");
        Console.WriteLine("  DeleteAsync   → ExecuteNonQueryAsync");
        Console.WriteLine();
    }
}
