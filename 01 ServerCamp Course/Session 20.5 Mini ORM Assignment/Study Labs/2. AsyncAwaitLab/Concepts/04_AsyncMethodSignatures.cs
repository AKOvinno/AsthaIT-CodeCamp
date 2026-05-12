// ================================================================
//  04 — Reading async Method Signatures
//
//  When you see an async method in MiniOrm, you need to know
//  how to read it, call it, and handle its return value.
//
//  This file shows every async signature pattern used in MiniOrm.
// ================================================================

namespace AsyncAwaitLab.Concepts;

public static class Concept04_AsyncMethodSignatures
{
    public static async Task RunAsync()
    {
        Console.WriteLine("=== 04: Reading async Method Signatures ===\n");

        // ── Pattern 1: Task<int> — used in InsertAsync ────────────
        Console.WriteLine("-- Pattern 1: Task<int> → InsertAsync --");
        Console.WriteLine("Signature: public async Task<int> InsertAsync(T entity)");
        Console.WriteLine("Call:      int id = await db.Products.InsertAsync(product)");
        Console.WriteLine();

        int insertedId = await FakeInsertAsync();
        Console.WriteLine($"Result: id = {insertedId}");
        Console.WriteLine();

        // ── Pattern 2: Task<T?> — used in FindByIdAsync ──────────
        Console.WriteLine("-- Pattern 2: Task<T?> → FindByIdAsync --");
        Console.WriteLine("Signature: public async Task<T?> FindByIdAsync(int id)");
        Console.WriteLine("Call:      var product = await db.Products.FindByIdAsync(1)");
        Console.WriteLine();

        string? found = await FakeFindByIdAsync(1);
        Console.WriteLine($"Found id=1: {found ?? "null"}");

        string? notFound = await FakeFindByIdAsync(99);
        Console.WriteLine($"Found id=99: {notFound ?? "null"}");
        Console.WriteLine();

        // ── Pattern 3: Task<List<T>> — used in GetAllAsync ────────
        Console.WriteLine("-- Pattern 3: Task<List<T>> → GetAllAsync --");
        Console.WriteLine("Signature: public async Task<List<T>> GetAllAsync()");
        Console.WriteLine("Call:      var all = await db.Products.GetAllAsync()");
        Console.WriteLine();

        List<string> all = await FakeGetAllAsync();
        Console.WriteLine($"Got {all.Count} item(s): {string.Join(", ", all)}");
        Console.WriteLine();

        // ── Pattern 4: Task — used in UpdateAsync, DeleteAsync ────
        Console.WriteLine("-- Pattern 4: Task (no return) → UpdateAsync / DeleteAsync --");
        Console.WriteLine("Signature: public async Task UpdateAsync(T entity)");
        Console.WriteLine("Call:      await db.Products.UpdateAsync(product)");
        Console.WriteLine();

        await FakeUpdateAsync();
        Console.WriteLine("UpdateAsync completed (no return value).");
        Console.WriteLine();

        // ── Pattern 5: Task in constructor helpers ────────────────
        Console.WriteLine("-- Pattern 5: Task → TestConnectionAsync --");
        Console.WriteLine("Signature: public async Task TestConnectionAsync()");
        Console.WriteLine("Call:      await db.TestConnectionAsync()");
        Console.WriteLine();

        await FakeTestConnectionAsync();
        Console.WriteLine();

        // ── Null checking after await ─────────────────────────────
        Console.WriteLine("-- Null checking the result of await --");
        Console.WriteLine("var found = await db.Products.FindByIdAsync(1);");
        Console.WriteLine("if (found != null) { ... }");
        Console.WriteLine("// or with null conditional:");
        Console.WriteLine("Console.WriteLine(found?.Name);");
        Console.WriteLine();

        var result = await FakeFindByIdAsync(1);
        Console.WriteLine($"found?.Length = {result?.Length}");  // safe — no crash if null
        Console.WriteLine();
    }

    private static async Task<int> FakeInsertAsync()
    {
        await Task.Delay(50);
        return 1;
    }

    private static async Task<string?> FakeFindByIdAsync(int id)
    {
        await Task.Delay(50);
        return id == 1 ? "Keyboard" : null;
    }

    private static async Task<List<string>> FakeGetAllAsync()
    {
        await Task.Delay(50);
        return ["Keyboard", "Mouse", "Monitor"];
    }

    private static async Task FakeUpdateAsync()
    {
        await Task.Delay(50);
        // no return value
    }

    private static async Task FakeTestConnectionAsync()
    {
        await Task.Delay(50);
        Console.WriteLine("Connection OK.");
    }
}
