using ReflectionLab.Models;

// ── Product model ─────────────────────────────────────────────
// Used across all concept files.
// Notice: IgnoredProp has no [Column] attribute — it will be
// skipped by the ORM because EntityMetadata ignores it.

[Table("products")]
public class Product
{
    [PrimaryKey]           public int      Id         { get; set; }
    [Column("name")]       public string   Name       { get; set; } = string.Empty;
    [Column("price")]      public decimal  Price      { get; set; }
    [Column("discount")]   public decimal? Discount   { get; set; }   // nullable value type
    [Column("in_stock")]   public bool     InStock    { get; set; }

    // No [Column] attribute → ORM skips this property entirely
    public string IgnoredProp { get; set; } = "I have no [Column]";
}
