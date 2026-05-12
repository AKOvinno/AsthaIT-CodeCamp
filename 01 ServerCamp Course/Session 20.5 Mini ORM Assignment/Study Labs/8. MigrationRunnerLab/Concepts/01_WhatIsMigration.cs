// ================================================================
//  01 — What is a Database Migration?
//
//  Your C# code changes over time. Your database schema must
//  change with it. Migrations are the controlled way to do that.
//
//  This file is pure explanation — no DB calls needed.
// ================================================================

namespace MigrationRunnerLab.Concepts;

public static class Concept01_WhatIsMigration
{
    public static void Run()
    {
        Console.WriteLine("=== 01: What is a Database Migration? ===\n");

        Console.WriteLine("A migration is a versioned change to your database schema.");
        Console.WriteLine("Each migration has two parts:\n");

        Console.WriteLine("  -- up   → what to DO   (CREATE TABLE, ADD COLUMN)");
        Console.WriteLine("  -- down → how to UNDO   (DROP TABLE, DROP COLUMN)");
        Console.WriteLine();

        // ── A real migration file ─────────────────────────────────
        Console.WriteLine("-- Example migration file: 20240506120000_InitialCreate.sql --");
        Console.WriteLine();
        Console.WriteLine("  -- up");
        Console.WriteLine("  CREATE TABLE IF NOT EXISTS products (");
        Console.WriteLine("      id       SERIAL PRIMARY KEY,");
        Console.WriteLine("      name     TEXT NOT NULL,");
        Console.WriteLine("      price    NUMERIC NOT NULL,");
        Console.WriteLine("      discount NUMERIC NULL");
        Console.WriteLine("  );");
        Console.WriteLine();
        Console.WriteLine("  -- down");
        Console.WriteLine("  DROP TABLE IF EXISTS products;");
        Console.WriteLine();

        // ── Why migrations instead of manual SQL? ─────────────────
        Console.WriteLine("-- Why not just run SQL manually? --");
        Console.WriteLine();
        Console.WriteLine("  Problem: You have 3 developers and 2 environments (dev + prod).");
        Console.WriteLine("  Without migrations:");
        Console.WriteLine("    → Developer A adds a column locally — forgets to tell others");
        Console.WriteLine("    → Developer B's app crashes — missing column");
        Console.WriteLine("    → Prod database is out of sync — nobody knows what state it's in");
        Console.WriteLine();
        Console.WriteLine("  With migrations:");
        Console.WriteLine("    → Every schema change is a tracked file in the repo");
        Console.WriteLine("    → Anyone can run 'migrations apply' and be in sync");
        Console.WriteLine("    → Applied migrations are recorded in __migrations table");
        Console.WriteLine("    → You can rollback if something goes wrong");
        Console.WriteLine();

        // ── The four commands ─────────────────────────────────────
        Console.WriteLine("-- MiniOrm Migration CLI commands --");
        Console.WriteLine();
        Console.WriteLine("  migrations add <Name>");
        Console.WriteLine("    → Generates a new timestamped .sql file");
        Console.WriteLine("    → Does NOT touch the database");
        Console.WriteLine("    → You review the file before applying");
        Console.WriteLine();
        Console.WriteLine("  migrations apply");
        Console.WriteLine("    → Finds all .sql files not yet in __migrations");
        Console.WriteLine("    → Runs their -- up section against Postgres");
        Console.WriteLine("    → Records each in __migrations table");
        Console.WriteLine();
        Console.WriteLine("  migrations list");
        Console.WriteLine("    → Shows every .sql file with [applied] or [pending]");
        Console.WriteLine();
        Console.WriteLine("  migrations rollback");
        Console.WriteLine("    → Runs -- down of the LAST applied migration");
        Console.WriteLine("    → Removes it from __migrations table");
        Console.WriteLine();
    }
}
