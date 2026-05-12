// ================================================================
//  01 — What is ADO.NET?
//
//  ADO.NET is the low-level .NET library for talking to databases.
//  It gives you raw control: you write the SQL, you manage the
//  connection, you read the results row by row.
//
//  EF Core sits ON TOP of ADO.NET — it generates SQL and calls
//  ADO.NET under the hood. MiniOrm does the same thing but simpler.
//
//  NOTE: This file has NO database calls — just explanations.
//        Files 02-06 have the actual code you run against Postgres.
// ================================================================

namespace AdoNetLab.Concepts;

public static class Concept01_WhatIsAdoNet
{
    public static void Run()
    {
        Console.WriteLine("=== 01: What is ADO.NET? ===\n");

        Console.WriteLine("ADO.NET is the foundation of ALL .NET database access.");
        Console.WriteLine();

        // ── The layers ────────────────────────────────────────────
        Console.WriteLine("-- The layers (top to bottom) --");
        Console.WriteLine("  Your Code");
        Console.WriteLine("      ↓");
        Console.WriteLine("  EF Core / Dapper / MiniOrm   ← ORMs");
        Console.WriteLine("      ↓");
        Console.WriteLine("  ADO.NET                       ← what MiniOrm uses directly");
        Console.WriteLine("      ↓");
        Console.WriteLine("  Npgsql                        ← PostgreSQL driver");
        Console.WriteLine("      ↓");
        Console.WriteLine("  PostgreSQL");
        Console.WriteLine();

        // ── The three core objects ─────────────────────────────────
        Console.WriteLine("-- The three ADO.NET objects you need --");
        Console.WriteLine();
        Console.WriteLine("  1. NpgsqlConnection");
        Console.WriteLine("     → Represents the connection to Postgres");
        Console.WriteLine("     → You open it, use it, close it");
        Console.WriteLine("     → Like a phone call — open the line, talk, hang up");
        Console.WriteLine();
        Console.WriteLine("  2. NpgsqlCommand");
        Console.WriteLine("     → Holds the SQL you want to run");
        Console.WriteLine("     → You add parameters to it (@p0, @p1...)");
        Console.WriteLine("     → You call Execute on it");
        Console.WriteLine("     → Like the message you send over that phone call");
        Console.WriteLine();
        Console.WriteLine("  3. NpgsqlDataReader");
        Console.WriteLine("     → Reads the rows returned by a SELECT");
        Console.WriteLine("     → You call Read() to move to the next row");
        Console.WriteLine("     → Like reading a response line by line");
        Console.WriteLine();

        // ── The three Execute methods ─────────────────────────────
        Console.WriteLine("-- The three Execute methods --");
        Console.WriteLine();
        Console.WriteLine("  ExecuteNonQueryAsync()");
        Console.WriteLine("     → For INSERT, UPDATE, DELETE");
        Console.WriteLine("     → Returns number of rows affected");
        Console.WriteLine("     → MiniOrm uses this in UpdateAsync and DeleteAsync");
        Console.WriteLine();
        Console.WriteLine("  ExecuteScalarAsync()");
        Console.WriteLine("     → For queries that return ONE value");
        Console.WriteLine("     → MiniOrm uses this in InsertAsync (RETURNING id)");
        Console.WriteLine("     → Returns the first column of the first row");
        Console.WriteLine();
        Console.WriteLine("  ExecuteReaderAsync()");
        Console.WriteLine("     → For SELECT queries that return rows");
        Console.WriteLine("     → Returns a NpgsqlDataReader");
        Console.WriteLine("     → MiniOrm uses this in FindByIdAsync and GetAllAsync");
        Console.WriteLine();

        Console.WriteLine("-- IMPORTANT --");
        Console.WriteLine("Files 02-06 require a running PostgreSQL database.");
        Console.WriteLine("Set MINIORM_CONN before running them.");
        Console.WriteLine("If you don't have Postgres yet, read the files — the");
        Console.WriteLine("comments explain every line thoroughly.");
        Console.WriteLine();
    }
}
