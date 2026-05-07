using System.Reflection;
using MiniOrm.Attributes;

namespace MiniOrm.Data;

/// <summary>Holds pre-computed reflection metadata for a single entity type.</summary>
public sealed class EntityMetadata<T>
{
    public string TableName { get; }
    public PropertyInfo PrimaryKey { get; }
    public string PrimaryKeyColumn { get; }
    public IReadOnlyList<PropertyInfo> Columns { get; }          // non-PK mapped columns
    public IReadOnlyList<string> ColumnNames { get; }

    public EntityMetadata()
    {
        var type = typeof(T);

        var tableAttr = type.GetCustomAttribute<TableAttribute>()
            ?? throw new InvalidOperationException($"{type.Name} is missing [Table] attribute.");
        TableName = tableAttr.Name;

        var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var pkProp = props.FirstOrDefault(p => p.GetCustomAttribute<PrimaryKeyAttribute>() != null)
            ?? throw new InvalidOperationException($"{type.Name} has no [PrimaryKey] property.");
        PrimaryKey = pkProp;
        PrimaryKeyColumn = "id"; // convention: PK column is always "id"

        var cols = new List<PropertyInfo>();
        var colNames = new List<string>();
        foreach (var prop in props)
        {
            if (prop == pkProp) continue;
            var colAttr = prop.GetCustomAttribute<ColumnAttribute>();
            if (colAttr == null) continue;
            cols.Add(prop);
            colNames.Add(colAttr.Name);
        }
        Columns = cols;
        ColumnNames = colNames;
    }
}
