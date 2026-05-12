// ================================================================
//  05 — DBNull.Value
//
//  DBNull is NOT the same as C# null.
//  It is a special object that represents NULL in a database.
//  Npgsql uses DBNull.Value — not C# null — for NULL columns.
//
//  MiniOrm must convert between C# null and DBNull.Value
//  in both directions: sending data TO postgres and reading FROM it.
// ================================================================

namespace NullableLab.Concepts;

public static class Concept05_DBNull
{
    public static void Run()
    {
        Console.WriteLine("=== 05: DBNull.Value ===\n");

        // ── What is DBNull? ───────────────────────────────────────
        // DBNull is a class with a single instance: DBNull.Value
        // It represents "this column has no value in the database"

        object dbNull  = DBNull.Value;
        object csNull  = null!;

        Console.WriteLine($"DBNull.Value type : {dbNull.GetType().Name}");  // DBNull
        Console.WriteLine($"DBNull == null    : {dbNull == null}");          // False!
        Console.WriteLine($"DBNull is DBNull  : {dbNull is DBNull}");        // True

        Console.WriteLine();

        // ── Direction 1: C# → Postgres (sending null to DB) ──────
        // When you have a nullable property with null value,
        // you cannot send C# null to Npgsql — you must send DBNull.Value

        Console.WriteLine("-- C# null → DBNull.Value (sending to Postgres) --");
        decimal? discount = null;

        // WRONG — Npgsql will throw if you pass C# null
        // cmd.Parameters.AddWithValue("discount", discount);  // ERROR

        // CORRECT — convert null to DBNull.Value first
        object paramValue = (object?)discount ?? DBNull.Value;
        Console.WriteLine($"Param sent to Npgsql: {paramValue}");  // System.DBNull

        discount = 5.00m;
        paramValue = (object?)discount ?? DBNull.Value;
        Console.WriteLine($"Param sent to Npgsql: {paramValue}");  // 5.00

        // This is exactly what DbSet.AddParams() does:
        // var value = _meta.Columns[i].GetValue(entity);
        // cmd.Parameters.AddWithValue($"p{i}", value ?? DBNull.Value);

        Console.WriteLine();

        // ── Direction 2: Postgres → C# (reading null from DB) ────
        // When Npgsql reads a NULL column, it returns DBNull.Value
        // You must convert DBNull.Value back to C# null

        Console.WriteLine("-- DBNull.Value → C# null (reading from Postgres) --");

        // Simulating what NpgsqlDataReader returns for a NULL column
        object fromReader = DBNull.Value;

        // WRONG — assigning DBNull.Value to decimal? will fail
        // decimal? readDiscount = (decimal?)fromReader;  // InvalidCastException

        // CORRECT — check for DBNull first
        decimal? readDiscount = fromReader == DBNull.Value ? null : (decimal?)fromReader;
        Console.WriteLine($"Read from DB: {readDiscount?.ToString() ?? "null"}");  // null

        // Simulating a non-null column
        fromReader = 5.00m;
        readDiscount = fromReader == DBNull.Value ? null : (decimal?)fromReader;
        Console.WriteLine($"Read from DB: {readDiscount}");  // 5.00

        // This is exactly what DbSet.Map() does:
        // var raw = reader[_meta.ColumnNames[i]];
        // _meta.Columns[i].SetValue(entity, raw == DBNull.Value ? null : raw);

        Console.WriteLine();

        // ── Summary: the two conversions ─────────────────────────
        Console.WriteLine("-- Summary --");
        Console.WriteLine("C# null       → DBNull.Value   (when sending to Npgsql)");
        Console.WriteLine("DBNull.Value  → C# null        (when reading from reader)");
        Console.WriteLine("Both done with: value ?? DBNull.Value  and  raw == DBNull.Value");
        Console.WriteLine();
    }
}
