using System.Reflection;
using MiniOrm.Attributes;

namespace MiniOrm.Data;

public static class TypeMapper
{
    /// <summary>Maps a C# property type to its PostgreSQL equivalent.</summary>
    public static string ToPostgresType(PropertyInfo property, bool isPrimaryKey)
    {
        if (isPrimaryKey) return "SERIAL PRIMARY KEY";

        var type = property.PropertyType;
        var nullableUnderlying = Nullable.GetUnderlyingType(type);

        // Nullable value type (e.g. int?)
        if (nullableUnderlying != null)
            return $"{MapBaseType(nullableUnderlying)} NULL";

        // Reference type string
        if (type == typeof(string))
        {
            // Detect nullable reference type via NullabilityInfoContext
            var ctx = new NullabilityInfoContext();
            var info = ctx.Create(property);
            bool isNullable = info.WriteState == NullabilityState.Nullable;
            return isNullable ? "TEXT NULL" : "TEXT NOT NULL";
        }

        // Non-nullable value type
        return $"{MapBaseType(type)} NOT NULL";
    }

    private static string MapBaseType(Type type) => type switch
    {
        _ when type == typeof(int)      => "INTEGER",
        _ when type == typeof(long)     => "BIGINT",
        _ when type == typeof(float)    => "REAL",
        _ when type == typeof(double)   => "DOUBLE PRECISION",
        _ when type == typeof(decimal)  => "NUMERIC",
        _ when type == typeof(bool)     => "BOOLEAN",
        _ when type == typeof(DateTime) => "TIMESTAMP",
        _ when type == typeof(Guid)     => "UUID",
        _ => throw new NotSupportedException($"Type '{type.Name}' is not supported.")
    };
}
