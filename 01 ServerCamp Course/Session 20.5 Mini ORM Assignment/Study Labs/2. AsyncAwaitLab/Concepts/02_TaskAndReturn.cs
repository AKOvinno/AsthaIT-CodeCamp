// ================================================================
//  02 — Task, Task<T>, and Return Values
//
//  async methods must return Task or Task<T>.
//  Task     = "I will finish eventually, no return value"
//  Task<T>  = "I will finish eventually and return a T"
//
//  In MiniOrm:
//    InsertAsync   → Task<int>      returns the new id
//    FindByIdAsync → Task<T?>       returns entity or null
//    GetAllAsync   → Task<List<T>>  returns all rows
//    UpdateAsync   → Task           no return value
//    DeleteAsync   → Task           no return value
// ================================================================

namespace AsyncAwaitLab.Concepts;

public static class Concept02_TaskAndReturn
{
    public static async Task RunAsync()
    {
        Console.WriteLine("=== 02: Task, Task<T>, and Return Values ===\n");

        // ── Task — no return value ────────────────────────────────
        // Like void but async. You await it to know it's done.

        Console.WriteLine("-- Task (no return value) --");
        await DoSomethingAsync();    // just wait for it to complete
        Console.WriteLine("DoSomethingAsync completed.");
        Console.WriteLine();

        // ── Task<T> — returns a value ─────────────────────────────
        // You await it and get the result directly.

        Console.WriteLine("-- Task<int> --");
        int id = await GetIdAsync();
        Console.WriteLine($"Got id: {id}");

        Console.WriteLine("-- Task<string> --");
        string name = await GetNameAsync();
        Console.WriteLine($"Got name: {name}");

        Console.WriteLine("-- Task<T?> (nullable) --");
        string? maybeName = await FindNameAsync(99);  // not found
        Console.WriteLine($"Found: {maybeName ?? "null"}");

        Console.WriteLine();

        // ── Awaiting stores the result ────────────────────────────
        // "await" unwraps the Task<T> and gives you the T directly
        // Without await: Task<int>  (a promise)
        // With    await: int        (the actual value)

        Console.WriteLine("-- await unwraps the value --");
        Task<int> taskWithoutAwait = GetIdAsync();     // Task<int>  — not awaited yet
        int       resultWithAwait  = await GetIdAsync(); // int       — awaited, unwrapped

        Console.WriteLine($"taskWithoutAwait type : {taskWithoutAwait.GetType().Name}");
        Console.WriteLine($"resultWithAwait  value: {resultWithAwait}");
        Console.WriteLine();

        // ── MiniOrm patterns ─────────────────────────────────────
        Console.WriteLine("-- MiniOrm return types --");
        Console.WriteLine("  InsertAsync   → Task<int>      → var id = await db.Products.InsertAsync(p)");
        Console.WriteLine("  FindByIdAsync → Task<T?>       → var p  = await db.Products.FindByIdAsync(1)");
        Console.WriteLine("  GetAllAsync   → Task<List<T>>  → var all = await db.Products.GetAllAsync()");
        Console.WriteLine("  UpdateAsync   → Task           → await db.Products.UpdateAsync(p)");
        Console.WriteLine("  DeleteAsync   → Task           → await db.Products.DeleteAsync(1)");
        Console.WriteLine();
    }

    // Task — async void equivalent (but proper)
    private static async Task DoSomethingAsync()
    {
        await Task.Delay(100);
        // no return statement needed
    }

    // Task<int> — returns an int asynchronously
    private static async Task<int> GetIdAsync()
    {
        await Task.Delay(100);
        return 42;   // this becomes the Task<int> result
    }

    // Task<string> — returns a string asynchronously
    private static async Task<string> GetNameAsync()
    {
        await Task.Delay(100);
        return "Keyboard";
    }

    // Task<T?> — returns nullable T (null when not found)
    private static async Task<string?> FindNameAsync(int id)
    {
        await Task.Delay(100);
        // Simulating "not found" scenario
        return null;
    }
}
