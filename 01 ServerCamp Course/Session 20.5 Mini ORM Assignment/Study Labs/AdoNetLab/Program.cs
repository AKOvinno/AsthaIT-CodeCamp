// ================================================================
//  AdoNetLab — Program.cs
//
//  SETUP BEFORE RUNNING:
//  1. Make sure PostgreSQL is running
//  2. Run 00_Setup.sql in your Postgres database
//  3. Set the environment variable:
//     export MINIORM_CONN="Host=localhost;Database=miniorm;Username=postgres;Password=secret"
// ================================================================

using AdoNetLab.Concepts;

Console.WriteLine("╔══════════════════════════════════════╗");
Console.WriteLine("║   AdoNetLab — Learning Project       ║");
Console.WriteLine("╚══════════════════════════════════════╝");
Console.WriteLine();

// Concept 01 has no DB calls — always safe to run
Concept01_WhatIsAdoNet.Run();

// Concepts 02-06 need a real Postgres connection
var connStr = Environment.GetEnvironmentVariable("MINIORM_CONN");

if (connStr == null)
{
    Console.WriteLine("╔══════════════════════════════════════════════════════╗");
    Console.WriteLine("║  MINIORM_CONN is not set.                            ║");
    Console.WriteLine("║  Concepts 02-06 require a Postgres connection.       ║");
    Console.WriteLine("║                                                      ║");
    Console.WriteLine("║  Set it with:                                        ║");
    Console.WriteLine("║  export MINIORM_CONN=\"Host=localhost;Database=...\"   ║");
    Console.WriteLine("║                                                      ║");
    Console.WriteLine("║  Also run 00_Setup.sql in your database first.       ║");
    Console.WriteLine("╚══════════════════════════════════════════════════════╝");
    return;
}

await Concept02_Connection.RunAsync(connStr);
await Concept03_Command.RunAsync(connStr);
await Concept04_ExecuteMethods.RunAsync(connStr);
await Concept05_DataReader.RunAsync(connStr);
await Concept06_MiniOrmDbSetWalkthrough.RunAsync(connStr);

Console.WriteLine("╔══════════════════════════════════════╗");
Console.WriteLine("║   AdoNetLab complete!                ║");
Console.WriteLine("║   DbSet<T> ADO.NET calls are clear   ║");
Console.WriteLine("╚══════════════════════════════════════╝");
