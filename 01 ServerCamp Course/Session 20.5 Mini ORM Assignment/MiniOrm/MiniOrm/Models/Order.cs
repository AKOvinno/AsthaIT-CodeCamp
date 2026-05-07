using MiniOrm.Attributes;

namespace MiniOrm.Models;

[Table("orders")]
public class Order
{
    [PrimaryKey]           public int Id { get; set; }
    [Column("product_id")] public int ProductId { get; set; }
    [Column("quantity")]   public int Quantity { get; set; }
    [Column("total")]      public decimal Total { get; set; }
    [Column("note")]       public string? Note { get; set; }      // nullable reference type
    [Column("placed_at")]  public DateTime PlacedAt { get; set; }
}
