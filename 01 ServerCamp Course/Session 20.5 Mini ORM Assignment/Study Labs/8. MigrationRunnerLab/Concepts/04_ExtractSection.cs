// ================================================================
//  04 — Extracting -- up and -- down Sections
//
//  Before applying or rolling back, MigrationRunner must extract
//  just the SQL it needs from the file.
//  ExtractSection() does this by scanning lines for markers.
//
//  No database required — this is pure string processing.
// ================================================================

namespace MigrationRunnerLab.Concepts;

public static class Concept04_ExtractSection
{
    public static void Run()
    {
        Console.WriteLine("=== 04: Extracting -- up and -- down Sections ===\n");

        string migrationFile =
            "-- up\n" +
            "CREATE TABLE IF NOT EXISTS products (\n" +
            "    id    SERIAL PRIMARY KEY,\n" +
            "    name  TEXT NOT NULL,\n" +
            "    price NUMERIC NOT NULL\n" +
            ");\n" +
            "\n" +
            "CREATE TABLE IF NOT EXISTS orders (\n" +
            "    id       SERIAL PRIMARY KEY,\n" +
            "    quantity INTEGER NOT NULL\n" +
            ");\n" +
            "\n" +
            "-- down\n" +
            "DROP TABLE IF EXISTS orders;\n" +
            "DROP TABLE IF EXISTS products;\n";

        // ── Show the raw file ─────────────────────────────────────
        Console.WriteLine("-- Raw file content --");
        Console.WriteLine(migrationFile);

        // ── Extract -- up ─────────────────────────────────────────
        Console.WriteLine("-- Extracted 'up' section --");
        string upSql = ExtractSection(migrationFile, "up");
        Console.WriteLine(upSql);
        Console.WriteLine();

        // ── Extract -- down ───────────────────────────────────────
        Console.WriteLine("-- Extracted 'down' section --");
        string downSql = ExtractSection(migrationFile, "down");
        Console.WriteLine(downSql);
        Console.WriteLine();

        // ── Walk through the algorithm line by line ───────────────
        Console.WriteLine("-- Algorithm walkthrough --");
        WalkThrough(migrationFile, "up");
        Console.WriteLine();
    }

    // ── Exact copy of MigrationRunner.ExtractSection() ───────────
    private static string ExtractSection(string content, string section)
    {
        string[] lines  = content.Split('\n');
        bool     capture = false;
        var      result  = new List<string>();

        foreach (var line in lines)
        {
            // Hit "-- up" or "-- down" matching our section → start capturing
            if (line.TrimStart().StartsWith($"-- {section}"))
            {
                capture = true;
                continue;    // skip the marker line itself
            }

            // Hit ANY other "-- " marker while capturing → stop
            if (line.TrimStart().StartsWith("-- ") && capture)
                break;

            // Collecting lines
            if (capture)
                result.Add(line);
        }

        return string.Join('\n', result).Trim();
    }

    // ── Verbose version that explains each decision ───────────────
    private static void WalkThrough(string content, string section)
    {
        Console.WriteLine($"Walking through file looking for '-- {section}':");
        string[] lines  = content.Split('\n');
        bool     capture = false;
        var      result  = new List<string>();

        foreach (var line in lines)
        {
            string display = line.Length > 40 ? line[..40] + "..." : line;

            if (line.TrimStart().StartsWith($"-- {section}"))
            {
                Console.WriteLine($"  → Found marker '-- {section}': START capturing");
                capture = true;
                continue;
            }

            if (line.TrimStart().StartsWith("-- ") && capture)
            {
                Console.WriteLine($"  → Found next marker: STOP capturing");
                break;
            }

            if (capture)
            {
                result.Add(line);
                if (display.Trim().Length > 0)
                    Console.WriteLine($"  + Captured: '{display}'");
            }
            else
            {
                if (display.Trim().Length > 0)
                    Console.WriteLine($"  - Skipped:  '{display}'");
            }
        }

        Console.WriteLine($"  Result: {result.Count} lines captured");
    }
}
