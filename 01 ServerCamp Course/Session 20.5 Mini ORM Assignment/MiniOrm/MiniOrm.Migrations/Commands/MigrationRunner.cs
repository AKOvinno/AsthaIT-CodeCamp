using System.Reflection;
using MiniOrm.Attributes;
using MiniOrm.Data;
using MiniOrm.Models;
using Npgsql;

namespace MiniOrm.Migrations.Commands;

/// <summary>
/// Handles add / apply / list / rollback migration commands.
/// </summary>
public sealed class MigrationRunner
{
    private const string MigrationsTable = "__migrations";
    private const string MigrationsFolder = "Migrations";

    private readonly string _connStr;

    // All entity types the ORM knows about – extend this list when you add new models.
    private static readonly IReadOnlyList<Type> RegisteredEntities =
    [
        typeof(Product),
        typeof(Order)
    ];

    public MigrationRunner(string connStr) => _connStr = connStr;

    // ── add ──────────────────────────────────────────────────────────────────

    public void Add(string name)
    {
        Directory.CreateDirectory(MigrationsFolder);
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var fileName = $"{timestamp}_{name}.sql";
        var path = Path.Combine(MigrationsFolder, fileName);

        var upStatements   = new List<string>();
        var downStatements = new List<string>();

        foreach (var entityType in RegisteredEntities)
        {
            var (tableName, columns) = BuildColumnDefs(entityType);
            upStatements.Add(BuildCreateTable(tableName, columns));
            downStatements.Add($"DROP TABLE IF EXISTS {tableName};");
        }

        var content = "-- up\n" +
                      string.Join("\n\n", upStatements) +
                      "\n\n-- down\n" +
                      string.Join("\n", downStatements) + "\n";

        File.WriteAllText(path, content);
        Console.WriteLine($"Created migration: {fileName}");
    }

    // ── apply ────────────────────────────────────────────────────────────────

    public void Apply()
    {
        EnsureMigrationsTable();
        var applied = GetAppliedMigrations();
        var pending = GetAllMigrationFiles()
            .Where(f => !applied.Contains(Path.GetFileName(f)))
            .OrderBy(f => f)
            .ToList();

        if (pending.Count == 0) { Console.WriteLine("No pending migrations."); return; }

        using var conn = Open();
        foreach (var file in pending)
        {
            var sql = ExtractSection(File.ReadAllText(file), "up");
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.ExecuteNonQuery();

            RecordMigration(conn, Path.GetFileName(file));
            Console.WriteLine($"Applied: {Path.GetFileName(file)}");
        }
    }

    // ── list ─────────────────────────────────────────────────────────────────

    public void List()
    {
        EnsureMigrationsTable();
        var applied = GetAppliedMigrations();
        var files = GetAllMigrationFiles().OrderBy(f => f).ToList();

        if (files.Count == 0) { Console.WriteLine("No migration files found."); return; }

        foreach (var file in files)
        {
            var name   = Path.GetFileName(file);
            var status = applied.Contains(name) ? "[applied]" : "[pending]";
            Console.WriteLine($"{status} {name}");
        }
    }

    // ── rollback ─────────────────────────────────────────────────────────────

    public void Rollback()
    {
        EnsureMigrationsTable();
        var last = GetLastAppliedMigration();
        if (last == null) { Console.WriteLine("Nothing to rollback."); return; }

        var path = Path.Combine(MigrationsFolder, last);
        if (!File.Exists(path))
            throw new FileNotFoundException($"Migration file not found: {path}");

        var sql = ExtractSection(File.ReadAllText(path), "down");
        using var conn = Open();
        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.ExecuteNonQuery();

        DeleteMigrationRecord(conn, last);
        Console.WriteLine($"Rolled back: {last}");
    }

    // ── Private helpers ───────────────────────────────────────────────────────

    private void EnsureMigrationsTable()
    {
        using var conn = Open();
        var sql = $"""
            CREATE TABLE IF NOT EXISTS {MigrationsTable} (
                id         SERIAL PRIMARY KEY,
                file_name  TEXT NOT NULL UNIQUE,
                applied_at TIMESTAMP NOT NULL DEFAULT NOW()
            );
            """;
        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.ExecuteNonQuery();
    }

    private HashSet<string> GetAppliedMigrations()
    {
        var result = new HashSet<string>();
        try
        {
            using var conn = Open();
            using var cmd = new NpgsqlCommand($"SELECT file_name FROM {MigrationsTable}", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) result.Add(reader.GetString(0));
        }
        catch { /* table may not exist yet */ }
        return result;
    }

    private string? GetLastAppliedMigration()
    {
        using var conn = Open();
        using var cmd = new NpgsqlCommand(
            $"SELECT file_name FROM {MigrationsTable} ORDER BY id DESC LIMIT 1", conn);
        return cmd.ExecuteScalar() as string;
    }

    private void RecordMigration(NpgsqlConnection conn, string fileName)
    {
        using var cmd = new NpgsqlCommand(
            $"INSERT INTO {MigrationsTable} (file_name) VALUES (@fn)", conn);
        cmd.Parameters.AddWithValue("@fn", fileName);
        cmd.ExecuteNonQuery();
    }

    private void DeleteMigrationRecord(NpgsqlConnection conn, string fileName)
    {
        using var cmd = new NpgsqlCommand(
            $"DELETE FROM {MigrationsTable} WHERE file_name = @fn", conn);
        cmd.Parameters.AddWithValue("@fn", fileName);
        cmd.ExecuteNonQuery();
    }

    private static IEnumerable<string> GetAllMigrationFiles() =>
        Directory.Exists(MigrationsFolder)
            ? Directory.GetFiles(MigrationsFolder, "*.sql")
            : [];

    /// Extracts the SQL between "-- up" / "-- down" markers.
    private static string ExtractSection(string content, string section)
    {
        var lines = content.Split('\n');
        var capture = false;
        var result  = new List<string>();

        foreach (var line in lines)
        {
            if (line.TrimStart().StartsWith($"-- {section}"))  { capture = true;  continue; }
            if (line.TrimStart().StartsWith("-- ") && capture) { break; }
            if (capture) result.Add(line);
        }
        return string.Join('\n', result).Trim();
    }

    // ── DDL builders ──────────────────────────────────────────────────────────

    private static (string tableName, List<string> columnDefs) BuildColumnDefs(Type entityType)
    {
        var tableAttr = entityType.GetCustomAttribute<TableAttribute>()
            ?? throw new InvalidOperationException($"{entityType.Name} missing [Table].");

        var props = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var columnDefs = new List<string>();

        foreach (var prop in props)
        {
            var isPk  = prop.GetCustomAttribute<PrimaryKeyAttribute>() != null;
            var colAttr = prop.GetCustomAttribute<ColumnAttribute>();

            if (!isPk && colAttr == null) continue;   // skip unmapped props

            var colName = isPk ? "id" : colAttr!.Name;
            var pgType  = TypeMapper.ToPostgresType(prop, isPk);
            columnDefs.Add($"    {colName} {pgType}");
        }

        return (tableAttr.Name, columnDefs);
    }

    private static string BuildCreateTable(string tableName, List<string> columnDefs) =>
        $"CREATE TABLE IF NOT EXISTS {tableName} (\n" +
        string.Join(",\n", columnDefs) +
        "\n);";

    private NpgsqlConnection Open()
    {
        var conn = new NpgsqlConnection(_connStr);
        conn.Open();
        return conn;
    }
}
