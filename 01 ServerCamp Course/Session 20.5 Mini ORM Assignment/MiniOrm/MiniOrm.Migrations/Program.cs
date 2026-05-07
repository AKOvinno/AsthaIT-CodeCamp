// ============================================================
//  MiniOrm.Migrations – CLI entry point (Task 04)
//  Usage:
//    dotnet run -- migrations add <Name>
//    dotnet run -- migrations apply
//    dotnet run -- migrations list
//    dotnet run -- migrations rollback
// ============================================================
using MiniOrm.Migrations.Commands;

var connStr = Environment.GetEnvironmentVariable("MINIORM_CONN")
    ?? throw new InvalidOperationException(
        "Set MINIORM_CONN environment variable.\n" +
        "Example: export MINIORM_CONN=\"Host=localhost;Database=miniorm;Username=postgres;Password=secret\"");

if (args.Length < 2 || args[0] != "migrations")
{
    PrintUsage();
    return;
}

var runner = new MigrationRunner(connStr);

switch (args[1])
{
    case "add":
        if (args.Length < 3) { Console.Error.WriteLine("Usage: migrations add <Name>"); return; }
        runner.Add(args[2]);
        break;
    case "apply":
        runner.Apply();
        break;
    case "list":
        runner.List();
        break;
    case "rollback":
        runner.Rollback();
        break;
    default:
        Console.Error.WriteLine($"Unknown command: {args[1]}");
        PrintUsage();
        break;
}

static void PrintUsage()
{
    Console.WriteLine("""
        MiniOrm Migration CLI
        ---------------------
        migrations add <Name>   Generate a new timestamped .sql migration file
        migrations apply        Apply all pending migrations
        migrations list         Show applied / pending migrations
        migrations rollback     Revert the last applied migration
        """);
}
