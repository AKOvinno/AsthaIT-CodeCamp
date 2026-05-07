using Npgsql;

namespace MiniOrm.Data;

/// <summary>
/// Base class for database contexts. Manages the connection string and
/// exposes a factory so derived classes can create DbSet&lt;T&gt; instances.
/// </summary>
public abstract class DbContext
{
    protected readonly string ConnectionString;

    protected DbContext(string connStr)
    {
        ConnectionString = connStr;
        InitializeSets();
    }

    /// <summary>Creates a DbSet bound to this context's connection string.</summary>
    protected DbSet<T> Set<T>() where T : new() => new(ConnectionString);

    /// <summary>Opens and immediately disposes a connection to verify credentials.</summary>
    public void TestConnection()
    {
        using var conn = new NpgsqlConnection(ConnectionString);
        conn.Open();
        Console.WriteLine("Connection OK.");
    }

    /// Scans derived class properties of type DbSet<T> and initialises them via reflection.
    private void InitializeSets()
    {
        var setType = typeof(DbSet<>);
        foreach (var prop in GetType().GetProperties())
        {
            if (!prop.PropertyType.IsGenericType) continue;
            if (prop.PropertyType.GetGenericTypeDefinition() != setType) continue;

            var entityType = prop.PropertyType.GetGenericArguments()[0];
            var instance = Activator.CreateInstance(prop.PropertyType, ConnectionString)!;
            prop.SetValue(this, instance);
        }
    }
}
