// ================================================================
//  GenericsLab — Program.cs
//  Runs all 7 concepts in sequence.
//  Tip: comment out concepts you already understand and focus
//       on the ones that are still unclear.
// ================================================================

using GenericsLab.Concepts;

Console.WriteLine("╔══════════════════════════════════════╗");
Console.WriteLine("║   GenericsLab — Learning Project     ║");
Console.WriteLine("╚══════════════════════════════════════╝");
Console.WriteLine();

Concept01_WhyGenerics.Run();
Concept02_GenericClass.Run();
Concept03_GenericMethods.Run();
Concept04_Constraints.Run();
Concept05_GenericInterfaces.Run();
Concept06_DefaultAndTypeof.Run();
Concept07_MiniOrmDbSet.Run();

Console.WriteLine("╔══════════════════════════════════════╗");
Console.WriteLine("║      All concepts complete!          ║");
Console.WriteLine("║                                      ║");
Console.WriteLine("║  You now understand DbSet<T> fully.  ║");
Console.WriteLine("╚══════════════════════════════════════╝");
