// ================================================================
//  06 — async/await in MiniOrm End to End
//
//  This traces the full async call chain from Program.cs
//  all the way down through DbSet<T> — showing every
//  async/await in the exact order MiniOrm executes them.
//
//  No DB required — uses simulated delays.
// ================================================================

namespace AsyncAwaitLab.Concepts;

public static class Concept06_AsyncInMiniOrm
{
    public static async Task RunAsync()
    {
        Console.WriteLine("=== 06: async/await in MiniOrm End to End ===\n");

        Console.WriteLine("Simulating the full MiniOrm async call chain:");
        Console.WriteLine("Program.cs → AppDbContext → DbSet<T> → NpgsqlCommand");
        Console.WriteLine();

        // ── Simulate InsertAsync ───────────────────────────────────
        Console.WriteLine("--- InsertAsync call chain ---");
        int id = await SimulatedInsertAsync("Keyboard", 89.99m, null);
        Console.WriteLine($"Back in Program.cs: id = {id}");
        Console.WriteLine();

        // ── Simulate FindByIdAsync ─────────────────────────────────
        Console.WriteLine("--- FindByIdAsync call chain ---");
        string? found = await SimulatedFindByIdAsync(id);
        Console.WriteLine($"Back in Program.cs: found = {found ?? "null"}");
        Console.WriteLine();

        // ── Simulate UpdateAsync ───────────────────────────────────
        Console.WriteLine("--- UpdateAsync call chain ---");
        await SimulatedUpdateAsync(id, 79.99m);
        Console.WriteLine("Back in Program.cs: update complete.");
        Console.WriteLine();

        // ── Simulate DeleteAsync ───────────────────────────────────
        Console.WriteLine("--- DeleteAsync call chain ---");
        await SimulatedDeleteAsync(id);
        Console.WriteLine("Back in Program.cs: delete complete.");
        Console.WriteLine();

        // ── The async chain explained ─────────────────────────────
        Console.WriteLine("-- The async chain --");
        Console.WriteLine();
        Console.WriteLine("Program.cs");
        Console.WriteLine("  await db.Products.InsertAsync(product)");
        Console.WriteLine("      ↓");
        Console.WriteLine("  DbSet<T>.InsertAsync()");
        Console.WriteLine("    using var conn = new NpgsqlConnection(_connStr)");
        Console.WriteLine("    await conn.OpenAsync()              ← awaits network");
        Console.WriteLine("    using var cmd = new NpgsqlCommand(sql, conn)");
        Console.WriteLine("    AddParams(cmd, entity)");
        Console.WriteLine("    var id = await cmd.ExecuteScalarAsync()  ← awaits Postgres");
        Console.WriteLine("    return id");
        Console.WriteLine("      ↑");
        Console.WriteLine("  int id = [result unwrapped from Task<int>]");
        Console.WriteLine();
    }

    // Simulates DbSet<T>.InsertAsync() without a real DB
    private static async Task<int> SimulatedInsertAsync(
        string name, decimal price, decimal? discount)
    {
        Console.WriteLine("  [DbSet.InsertAsync] Building INSERT SQL...");
        string sql = $"INSERT INTO products (name, price, discount) " +
                     $"VALUES (@p0, @p1, @p2) RETURNING id";
        Console.WriteLine($"  [DbSet.InsertAsync] SQL: {sql}");

        Console.WriteLine("  [DbSet.InsertAsync] await conn.OpenAsync()");
        await Task.Delay(80);   // simulates network round-trip
        Console.WriteLine("  [DbSet.InsertAsync] Connection open");

        Console.WriteLine("  [DbSet.InsertAsync] await cmd.ExecuteScalarAsync()");
        await Task.Delay(120);  // simulates Postgres processing
        int newId = 1;          // simulated RETURNING id
        Console.WriteLine($"  [DbSet.InsertAsync] Got id={newId} from RETURNING");

        return newId;
    }

    // Simulates DbSet<T>.FindByIdAsync()
    private static async Task<string?> SimulatedFindByIdAsync(int id)
    {
        Console.WriteLine($"  [DbSet.FindByIdAsync] SELECT * WHERE id={id}");

        Console.WriteLine("  [DbSet.FindByIdAsync] await conn.OpenAsync()");
        await Task.Delay(80);

        Console.WriteLine("  [DbSet.FindByIdAsync] await cmd.ExecuteReaderAsync()");
        await Task.Delay(100);

        Console.WriteLine("  [DbSet.FindByIdAsync] await reader.ReadAsync()");
        await Task.Delay(30);

        Console.WriteLine("  [DbSet.FindByIdAsync] Mapping row to entity");
        return "Keyboard";   // simulated mapped entity
    }

    // Simulates DbSet<T>.UpdateAsync()
    private static async Task SimulatedUpdateAsync(int id, decimal newPrice)
    {
        Console.WriteLine($"  [DbSet.UpdateAsync] UPDATE ... WHERE id={id}");
        await Task.Delay(80);
        Console.WriteLine("  [DbSet.UpdateAsync] await conn.OpenAsync()");
        await Task.Delay(100);
        Console.WriteLine("  [DbSet.UpdateAsync] await cmd.ExecuteNonQueryAsync()");
        Console.WriteLine("  [DbSet.UpdateAsync] 1 row updated");
    }

    // Simulates DbSet<T>.DeleteAsync()
    private static async Task SimulatedDeleteAsync(int id)
    {
        Console.WriteLine($"  [DbSet.DeleteAsync] DELETE WHERE id={id}");
        await Task.Delay(80);
        Console.WriteLine("  [DbSet.DeleteAsync] await conn.OpenAsync()");
        await Task.Delay(80);
        Console.WriteLine("  [DbSet.DeleteAsync] await cmd.ExecuteNonQueryAsync()");
        Console.WriteLine($"  [DbSet.DeleteAsync] Deleted id={id} ✓");
    }
}
