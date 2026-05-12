// ================================================================
//  01 — Why Do We Need async/await?
//
//  Without async: your program FREEZES while waiting for the DB.
//  With async: your program stays responsive while waiting.
//
//  Every DbSet method in MiniOrm is async because all database
//  calls involve waiting — for the network, for Postgres to
//  process the query, for the result to come back.
//
//  No DB required — pure explanation with simulated delays.
// ================================================================

namespace AsyncAwaitLab.Concepts;

public static class Concept01_WhyAsync
{
    public static async Task RunAsync()
    {
        Console.WriteLine("=== 01: Why Do We Need async/await? ===\n");

        // ── The problem: synchronous waiting ──────────────────────
        // Imagine calling Thread.Sleep() to simulate a DB query.
        // While sleeping, the ENTIRE THREAD is blocked.
        // Nothing else can run. The program freezes.

        Console.WriteLine("-- Synchronous (blocking) --");
        Console.WriteLine("Starting sync work...");
        SyncWork();
        Console.WriteLine("Sync work done. Thread was BLOCKED the whole time.");
        Console.WriteLine();

        // ── The solution: async waiting ───────────────────────────
        // Task.Delay() is the async version of Thread.Sleep().
        // While "waiting", the thread is FREE to do other things.
        // In a web server, it could handle another request.
        // In MiniOrm, it waits for Postgres without blocking.

        Console.WriteLine("-- Asynchronous (non-blocking) --");
        Console.WriteLine("Starting async work...");
        await AsyncWork();
        Console.WriteLine("Async work done. Thread was FREE while waiting.");
        Console.WriteLine();

        // ── How this applies to MiniOrm ──────────────────────────
        Console.WriteLine("-- MiniOrm connection --");
        Console.WriteLine("Every DbSet method awaits the database call:");
        Console.WriteLine();
        Console.WriteLine("  await conn.OpenAsync()          → wait for connection");
        Console.WriteLine("  await cmd.ExecuteScalarAsync()  → wait for INSERT result");
        Console.WriteLine("  await cmd.ExecuteReaderAsync()  → wait for SELECT result");
        Console.WriteLine("  await cmd.ExecuteNonQueryAsync()→ wait for UPDATE/DELETE");
        Console.WriteLine();
        Console.WriteLine("Without async, MiniOrm would freeze while Postgres");
        Console.WriteLine("processes each query. With async, it waits efficiently.");
        Console.WriteLine();
    }

    // Synchronous — blocks the thread completely
    private static void SyncWork()
    {
        Console.WriteLine("  [Sync] Simulating DB query... (thread BLOCKED)");
        Thread.Sleep(500);   // blocks — nothing else can run
        Console.WriteLine("  [Sync] Query complete.");
    }

    // Asynchronous — frees the thread while waiting
    private static async Task AsyncWork()
    {
        Console.WriteLine("  [Async] Simulating DB query... (thread FREE)");
        await Task.Delay(500);   // non-blocking — thread is released
        Console.WriteLine("  [Async] Query complete.");
    }
}
