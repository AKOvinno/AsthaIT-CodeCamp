// ================================================================
//  03 — The async and await Keywords
//
//  async  → marks a method as asynchronous
//  await  → pauses the method until the Task completes,
//           then resumes with the result
//
//  Rules:
//  1. You can only use await inside an async method
//  2. Every async method must return Task, Task<T>, or void
//  3. async void is only for event handlers — never for normal code
// ================================================================

namespace AsyncAwaitLab.Concepts;

public static class Concept03_AsyncKeyword
{
    public static async Task RunAsync()
    {
        Console.WriteLine("=== 03: The async and await Keywords ===\n");

        // ── Basic async/await flow ────────────────────────────────
        Console.WriteLine("-- Basic flow --");
        Console.WriteLine("Before await");
        string result = await SimulateDbQueryAsync("SELECT name FROM products");
        Console.WriteLine($"After await: result = '{result}'");
        Console.WriteLine();

        // ── What happens at await ─────────────────────────────────
        // 1. The method hits "await"
        // 2. The Task starts running (DB query starts)
        // 3. The current method PAUSES here and returns control
        //    to whoever called it
        // 4. When the Task completes, the method RESUMES from
        //    exactly where it paused
        // 5. The result is unwrapped from Task<T> into T

        Console.WriteLine("-- Step by step --");
        await StepByStepAsync();
        Console.WriteLine();

        // ── await chains naturally ────────────────────────────────
        // You can await multiple things in sequence.
        // Each await pauses, then resumes before the next line.
        // This is exactly how MiniOrm's InsertAsync works.

        Console.WriteLine("-- Multiple awaits in sequence --");
        await MultipleAwaitsAsync();
        Console.WriteLine();

        // ── async propagates up the call stack ───────────────────
        // If a method uses await, it must be async.
        // If its caller awaits it, the caller must be async too.
        // This is why Program.cs uses "await" and main is async.

        Console.WriteLine("-- async propagates upward --");
        Console.WriteLine("DbSet.InsertAsync     → async Task<int>");
        Console.WriteLine("Program.cs calls it   → await db.Products.InsertAsync(p)");
        Console.WriteLine("So Program.cs must be → async (top-level statements handle this)");
        Console.WriteLine();
    }

    private static async Task<string> SimulateDbQueryAsync(string sql)
    {
        Console.WriteLine($"  → Query sent: '{sql}'");
        await Task.Delay(200);   // simulate waiting for Postgres
        Console.WriteLine($"  → Query complete");
        return "Keyboard";       // simulated result
    }

    private static async Task StepByStepAsync()
    {
        Console.WriteLine("  [1] Method starts");
        Console.WriteLine("  [2] Hit await — method pauses, Task starts");
        await Task.Delay(200);
        Console.WriteLine("  [3] Task completed — method resumes here");
        Console.WriteLine("  [4] Method continues normally");
    }

    private static async Task MultipleAwaitsAsync()
    {
        // Simulating: open connection, execute, read result
        Console.WriteLine("  await conn.OpenAsync()");
        await Task.Delay(100);
        Console.WriteLine("  Connection open ✓");

        Console.WriteLine("  await cmd.ExecuteScalarAsync()");
        await Task.Delay(150);
        Console.WriteLine("  Got result: id=1 ✓");

        Console.WriteLine("  await reader.ReadAsync()");
        await Task.Delay(100);
        Console.WriteLine("  Row read ✓");
    }
}
