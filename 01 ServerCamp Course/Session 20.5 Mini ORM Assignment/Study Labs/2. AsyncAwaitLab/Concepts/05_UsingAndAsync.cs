// ================================================================
//  05 — using var with async
//
//  MiniOrm uses "using var" for connections, commands, readers.
//  With async there's also "await using" for async disposal.
//
//  This file explains both and when to use each.
// ================================================================

namespace AsyncAwaitLab.Concepts;

public static class Concept05_UsingAndAsync
{
    public static async Task RunAsync()
    {
        Console.WriteLine("=== 05: using var with async ===\n");

        // ── using var — synchronous disposal ─────────────────────
        // Calls Dispose() automatically when block ends.
        // Works for anything that implements IDisposable.

        Console.WriteLine("-- using var (sync disposal) --");
        using var syncResource = new SyncResource("Connection");
        await syncResource.DoWorkAsync();
        Console.WriteLine("After using block — SyncResource.Dispose() was called");
        Console.WriteLine();

        // ── await using — async disposal ─────────────────────────
        // Calls DisposeAsync() automatically.
        // Used when the cleanup itself is async (like closing a network connection).

        Console.WriteLine("-- await using (async disposal) --");
        await using var asyncResource = new AsyncResource("AsyncConnection");
        await asyncResource.DoWorkAsync();
        Console.WriteLine("After await using — AsyncResource.DisposeAsync() was called");
        Console.WriteLine();

        // ── Which does MiniOrm use? ───────────────────────────────
        Console.WriteLine("-- What MiniOrm uses --");
        Console.WriteLine();
        Console.WriteLine("  DbSet methods use:  using var conn = new NpgsqlConnection(...)");
        Console.WriteLine("  This is sync disposal (IDisposable).");
        Console.WriteLine("  Npgsql's NpgsqlConnection implements both IDisposable");
        Console.WriteLine("  and IAsyncDisposable — using var works fine.");
        Console.WriteLine();
        Console.WriteLine("  Program.cs could use: await using var db = new AppDbContext(...)");
        Console.WriteLine("  This calls DisposeAsync() which closes the connection cleanly.");
        Console.WriteLine();

        // ── What happens without using ────────────────────────────
        Console.WriteLine("-- What happens WITHOUT using --");
        Console.WriteLine();
        Console.WriteLine("  var conn = new NpgsqlConnection(connStr);");
        Console.WriteLine("  await conn.OpenAsync();");
        Console.WriteLine("  // ... if exception thrown here ...");
        Console.WriteLine("  conn.Dispose();   ← NEVER REACHED if exception occurred!");
        Console.WriteLine();
        Console.WriteLine("  With using var: Dispose() is ALWAYS called, even on exception.");
        Console.WriteLine("  This prevents connection leaks.");
        Console.WriteLine();

        // ── Simulating disposal on exception ─────────────────────
        Console.WriteLine("-- using var protects even on exception --");
        try
        {
            using var resource = new SyncResource("ProtectedConn");
            Console.WriteLine("  Resource created and opened.");
            throw new Exception("Simulated DB error!");
            // Dispose() still called even though we never reach end of block
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  Exception caught: {ex.Message}");
            Console.WriteLine("  But SyncResource.Dispose() was still called ✓");
        }
        Console.WriteLine();
    }
}

// ── Sync disposable resource ──────────────────────────────────────
public class SyncResource(string name) : IDisposable
{
    public async Task DoWorkAsync()
    {
        Console.WriteLine($"  [{name}] Doing work...");
        await Task.Delay(50);
        Console.WriteLine($"  [{name}] Work done.");
    }

    public void Dispose()
    {
        Console.WriteLine($"  [{name}] Dispose() called — resource cleaned up.");
    }
}

// ── Async disposable resource ─────────────────────────────────────
public class AsyncResource(string name) : IAsyncDisposable
{
    public async Task DoWorkAsync()
    {
        Console.WriteLine($"  [{name}] Doing async work...");
        await Task.Delay(50);
        Console.WriteLine($"  [{name}] Work done.");
    }

    public async ValueTask DisposeAsync()
    {
        Console.WriteLine($"  [{name}] DisposeAsync() called...");
        await Task.Delay(20);   // async cleanup
        Console.WriteLine($"  [{name}] Cleaned up asynchronously.");
    }
}
