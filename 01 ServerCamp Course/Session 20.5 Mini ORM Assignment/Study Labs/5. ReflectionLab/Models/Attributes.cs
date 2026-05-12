namespace ReflectionLab.Models;

// ── Custom Attributes ─────────────────────────────────────────
// These are the same attributes used in MiniOrm.
// Defined here so all concept files can use them.

[AttributeUsage(AttributeTargets.Class)]
public sealed class TableAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class ColumnAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class PrimaryKeyAttribute : Attribute { }
