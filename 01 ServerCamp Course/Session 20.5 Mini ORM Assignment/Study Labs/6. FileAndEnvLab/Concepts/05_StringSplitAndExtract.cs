// ================================================================
//  05 — String Splitting and Section Extraction
//
//  MigrationRunner.ExtractSection() splits a .sql file by
//  "-- up" and "-- down" markers to get just the SQL it needs.
//
//  This file explains string splitting, which is the core
//  of how that method works.
// ================================================================

namespace FileAndEnvLab.Concepts;

public static class Concept05_StringSplitAndExtract
{
    public static void Run()
    {
        Console.WriteLine("=== 05: String Splitting and Section Extraction ===\n");

        // ── String.Split() ────────────────────────────────────────
        string csv = "apple,banana,cherry";
        string[] parts = csv.Split(',');

        Console.WriteLine("-- String.Split(',') --");
        foreach (var part in parts)
            Console.WriteLine($"  '{part}'");

        Console.WriteLine();

        // ── Split on newline ──────────────────────────────────────
        // MigrationRunner splits .sql file content into lines
        string multiLine = "line one\nline two\nline three";
        string[] lines = multiLine.Split('\n');

        Console.WriteLine("-- Split on newline --");
        foreach (var line in lines)
            Console.WriteLine($"  '{line}'");

        Console.WriteLine();

        // ── TrimStart() ───────────────────────────────────────────
        // Removes whitespace from the start of a string
        // MigrationRunner uses this to handle indented markers
        string indented = "    -- up";
        Console.WriteLine($"Original:   '{indented}'");
        Console.WriteLine($"TrimStart:  '{indented.TrimStart()}'");
        Console.WriteLine($"StartsWith: {indented.TrimStart().StartsWith("-- up")}");

        Console.WriteLine();

        // ── Building ExtractSection from scratch ──────────────────
        // This is the EXACT logic of MigrationRunner.ExtractSection()
        Console.WriteLine("-- ExtractSection() explained step by step --");

        string migrationFile =
            "-- up\n" +
            "CREATE TABLE IF NOT EXISTS products (\n" +
            "    id   SERIAL PRIMARY KEY,\n" +
            "    name TEXT NOT NULL\n" +
            ");\n\n" +
            "-- down\n" +
            "DROP TABLE IF EXISTS products;\n";

        Console.WriteLine("Extracting 'up' section:");
        Console.WriteLine(ExtractSection(migrationFile, "up"));
        Console.WriteLine();

        Console.WriteLine("Extracting 'down' section:");
        Console.WriteLine(ExtractSection(migrationFile, "down"));
        Console.WriteLine();
    }

    // Exact copy of MigrationRunner.ExtractSection()
    private static string ExtractSection(string content, string section)
    {
        string[] lines  = content.Split('\n');
        bool     capture = false;
        var      result  = new List<string>();

        foreach (var line in lines)
        {
            // Start capturing when we hit "-- up" or "-- down"
            if (line.TrimStart().StartsWith($"-- {section}"))
            {
                capture = true;
                continue;   // skip the marker line itself
            }

            // Stop capturing when we hit the NEXT marker
            if (line.TrimStart().StartsWith("-- ") && capture)
                break;

            // Collect lines while capturing
            if (capture)
                result.Add(line);
        }

        return string.Join('\n', result).Trim();
    }
}
