using ReflectionLab.Models;

// ── Order model ───────────────────────────────────────────────
// Notice the nullable reference type: string? Note
// This is different from decimal? — needs NullabilityInfoContext
// to detect at runtime (covered in 06_Nullable.cs)

[Table("orders")]
public class Order
{
    [PrimaryKey]             public int      Id        { get; set; }
    [Column("product_id")]   public int      ProductId { get; set; }
    [Column("quantity")]     public int      Quantity  { get; set; }
    [Column("note")]         public string?  Note      { get; set; }   // nullable reference type
    [Column("placed_at")]    public DateTime PlacedAt  { get; set; }
}
