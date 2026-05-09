// ================================================================
//  01 — Environment Variables
//
//  Environment variables are key-value pairs set OUTSIDE your app
//  at the OS level. They are used to pass configuration like
//  connection strings WITHOUT hardcoding them in source code.
//
//  MiniOrm reads MINIORM_CONN from the environment.
// ================================================================

namespace FileAndEnvLab.Concepts;

public static class Concept01_EnvironmentVariables
{
    public static void Run()
    {
        Console.WriteLine("=== 01: Environment Variables ===\n");

        // ── Reading an environment variable ───────────────────────
        // GetEnvironmentVariable returns null if the variable is not set
        string? path = Environment.GetEnvironmentVariable("PATH");
        Console.WriteLine($"PATH is set: {path != null}");

        string? missing = Environment.GetEnvironmentVariable("DOES_NOT_EXIST");
        Console.WriteLine($"MISSING is null: {missing == null}");

        Console.WriteLine();

        // ── Setting one for this process (for testing) ────────────
        // You can set variables in code — only for the current process
        Environment.SetEnvironmentVariable("MY_DB_CONN", "Host=localhost;Database=test");

        string? conn = Environment.GetEnvironmentVariable("MY_DB_CONN");
        Console.WriteLine($"MY_DB_CONN = {conn}");

        Console.WriteLine();

        // ── The MiniOrm pattern: read or throw ────────────────────
        // This is the exact pattern used in both Program.cs files:
        //
        // var connStr = Environment.GetEnvironmentVariable("MINIORM_CONN")
        //     ?? throw new InvalidOperationException("MINIORM_CONN is not set.");
        //
        // If the variable exists  → connStr gets the value
        // If the variable is null → immediately throws with a clear message

        Console.WriteLine("-- MiniOrm pattern --");

        // Simulating variable NOT set
        try
        {
            string? rawConn = null;   // simulating missing env var
            string connStr  = rawConn ?? throw new InvalidOperationException(
                "MINIORM_CONN environment variable is not set.\n" +
                "Set it with: export MINIORM_CONN=\"Host=localhost;...\"");

            Console.WriteLine($"connStr = {connStr}");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Caught: {ex.Message}");
        }

        Console.WriteLine();

        // Simulating variable IS set
        Environment.SetEnvironmentVariable("MINIORM_CONN",
            "Host=localhost;Database=miniorm;Username=postgres;Password=secret");

        string miniOrmConn = Environment.GetEnvironmentVariable("MINIORM_CONN")
            ?? throw new InvalidOperationException("Not set");

        Console.WriteLine($"MINIORM_CONN = {miniOrmConn}");

        Console.WriteLine();

        // ── How to set it in your terminal ────────────────────────
        Console.WriteLine("-- How to set MINIORM_CONN in terminal --");
        Console.WriteLine("Linux/macOS:");
        Console.WriteLine("  export MINIORM_CONN=\"Host=localhost;Database=miniorm;Username=postgres;Password=secret\"");
        Console.WriteLine();
        Console.WriteLine("Windows PowerShell:");
        Console.WriteLine("  $env:MINIORM_CONN=\"Host=localhost;Database=miniorm;Username=postgres;Password=secret\"");
        Console.WriteLine();
    }
}
