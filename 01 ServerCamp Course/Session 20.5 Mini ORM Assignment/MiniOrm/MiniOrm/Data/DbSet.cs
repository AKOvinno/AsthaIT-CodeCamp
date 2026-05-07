using Npgsql;
using NpgsqlTypes;

namespace MiniOrm.Data;

/// <summary>Generic repository providing parameterised CRUD over a single table.</summary>
public sealed class DbSet<T> where T : new()
{
    private readonly string _connStr;
    private readonly EntityMetadata<T> _meta;

    public DbSet(string connStr)
    {
        _connStr = connStr;
        _meta = new EntityMetadata<T>();
    }

    // ── Insert ───────────────────────────────────────────────────────────────

    public int Insert(T entity)
    {
        var colList  = string.Join(", ", _meta.ColumnNames);
        var paramList = string.Join(", ", _meta.ColumnNames.Select((_, i) => $"@p{i}"));
        var sql = $"INSERT INTO {_meta.TableName} ({colList}) VALUES ({paramList}) RETURNING {_meta.PrimaryKeyColumn}";

        using var conn = Open();
        using var cmd = new NpgsqlCommand(sql, conn);
        AddColumnParams(cmd, entity);
        var id = (int)cmd.ExecuteScalar()!;
        _meta.PrimaryKey.SetValue(entity, id);
        Console.WriteLine($"Inserted {typeof(T).Name} Id={id}, " +
            string.Join(", ", _meta.Columns.Zip(_meta.ColumnNames, (p, n) =>
                $"{n}={p.GetValue(entity) ?? "NULL"}")));
        return id;
    }

    // ── FindById ─────────────────────────────────────────────────────────────

    public T? FindById(int id)
    {
        var sql = $"SELECT * FROM {_meta.TableName} WHERE {_meta.PrimaryKeyColumn} = @id";
        using var conn = Open();
        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", id);
        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return default;
        return Map(reader);
    }

    // ── GetAll ────────────────────────────────────────────────────────────────

    public IEnumerable<T> GetAll()
    {
        var sql = $"SELECT * FROM {_meta.TableName}";
        using var conn = Open();
        using var cmd = new NpgsqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();
        var results = new List<T>();
        while (reader.Read()) results.Add(Map(reader));
        return results;
    }

    // ── Update ────────────────────────────────────────────────────────────────

    public void Update(T entity)
    {
        var setClause = string.Join(", ", _meta.ColumnNames.Select((n, i) => $"{n} = @p{i}"));
        var sql = $"UPDATE {_meta.TableName} SET {setClause} WHERE {_meta.PrimaryKeyColumn} = @pkVal";

        using var conn = Open();
        using var cmd = new NpgsqlCommand(sql, conn);
        AddColumnParams(cmd, entity);
        cmd.Parameters.AddWithValue("@pkVal", _meta.PrimaryKey.GetValue(entity)!);
        cmd.ExecuteNonQuery();
        Console.WriteLine($"Updated {typeof(T).Name} Id={_meta.PrimaryKey.GetValue(entity)}, " +
            string.Join(", ", _meta.Columns.Zip(_meta.ColumnNames, (p, n) =>
                $"{n}={p.GetValue(entity) ?? "NULL"}")));
    }

    // ── Delete ────────────────────────────────────────────────────────────────

    public void Delete(int id)
    {
        var sql = $"DELETE FROM {_meta.TableName} WHERE {_meta.PrimaryKeyColumn} = @id";
        using var conn = Open();
        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
        Console.WriteLine($"Deleted {typeof(T).Name} Id={id} ✓");
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private NpgsqlConnection Open()
    {
        var conn = new NpgsqlConnection(_connStr);
        conn.Open();
        return conn;
    }

    /// Adds @p0, @p1 … parameters for every non-PK column.
    private void AddColumnParams(NpgsqlCommand cmd, T entity)
    {
        for (int i = 0; i < _meta.Columns.Count; i++)
        {
            var value = _meta.Columns[i].GetValue(entity);
            cmd.Parameters.AddWithValue($"@p{i}", value ?? DBNull.Value);
        }
    }

    /// Maps the current reader row to a new T instance.
    private T Map(NpgsqlDataReader reader)
    {
        var entity = new T();
        _meta.PrimaryKey.SetValue(entity, reader[_meta.PrimaryKeyColumn]);

        for (int i = 0; i < _meta.Columns.Count; i++)
        {
            var rawValue = reader[_meta.ColumnNames[i]];
            if (rawValue == DBNull.Value)
                _meta.Columns[i].SetValue(entity, null);
            else
                _meta.Columns[i].SetValue(entity, rawValue);
        }
        return entity;
    }
}
