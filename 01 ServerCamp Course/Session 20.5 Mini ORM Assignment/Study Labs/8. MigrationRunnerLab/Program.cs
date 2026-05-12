// ================================================================
//  MigrationRunnerLab — Program.cs
//
//  SETUP BEFORE RUNNING concepts 02, 05, 06, 07, 08:
//  1. PostgreSQL must be running
//  2. Set environment variable:
//     export MINIORM_CONN="Host=localhost;Database=miniorm;Username=postgres;Password=secret"
//
//  Concepts 01, 03, 04 need NO database — always safe to run.
// ================================================================

using MigrationRunnerLab.Concepts;

Console.WriteLine("╔════════════════════════════════════════════╗");
Console.WriteLine("║   MigrationRunnerLab — Learning Project    ║");
Console.WriteLine("╚════════════════════════════════════════════╝");
Console.WriteLine();

// ── No DB required ────────────────────────────────────────────────
Concept01_WhatIsMigration.Run();
Concept03_AddCommand.Run();
Concept04_ExtractSection.Run();

// ── DB required ───────────────────────────────────────────────────
var connStr = Environment.GetEnvironmentVariable("MINIORM_CONN");

if (connStr == null)
{
    Console.WriteLine("╔══════════════════════════════════════════════════════╗");
    Console.WriteLine("║  MINIORM_CONN is not set.                            ║");
    Console.WriteLine("║  Concepts 02, 05, 06, 07, 08 need Postgres.         ║");
    Console.WriteLine("║                                                      ║");
    Console.WriteLine("║  Set it and re-run to see the full migration         ║");
    Console.WriteLine("║  lifecycle against a real database.                  ║");
    Console.WriteLine("╚══════════════════════════════════════════════════════╝");
    return;
}

await Concept02_MigrationsTable.RunAsync(connStr);
await Concept05_ApplyCommand.RunAsync(connStr);
await Concept06_ListCommand.RunAsync(connStr);
await Concept07_RollbackCommand.RunAsync(connStr);
await Concept08_FullMigrationRunner.RunAsync(connStr);

Console.WriteLine("╔════════════════════════════════════════════╗");
Console.WriteLine("║   MigrationRunnerLab complete!             ║");
Console.WriteLine("║   MiniOrm migrations are fully clear now   ║");
Console.WriteLine("╚════════════════════════════════════════════╝");
