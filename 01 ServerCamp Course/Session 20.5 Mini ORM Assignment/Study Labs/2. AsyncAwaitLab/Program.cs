// ================================================================
//  AsyncAwaitLab — Program.cs
//  No database required — all concepts use simulated delays.
// ================================================================

using AsyncAwaitLab.Concepts;

Console.WriteLine("╔══════════════════════════════════════╗");
Console.WriteLine("║  AsyncAwaitLab — Learning Project    ║");
Console.WriteLine("╚══════════════════════════════════════╝");
Console.WriteLine();

await Concept01_WhyAsync.RunAsync();
await Concept02_TaskAndReturn.RunAsync();
await Concept03_AsyncKeyword.RunAsync();
await Concept04_AsyncMethodSignatures.RunAsync();
await Concept05_UsingAndAsync.RunAsync();
await Concept06_AsyncInMiniOrm.RunAsync();

Console.WriteLine("╔══════════════════════════════════════╗");
Console.WriteLine("║   AsyncAwaitLab complete!            ║");
Console.WriteLine("║   Every async line in MiniOrm is     ║");
Console.WriteLine("║   now clear.                         ║");
Console.WriteLine("╚══════════════════════════════════════╝");
