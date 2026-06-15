namespace MiniDukaan.Domain.Entities;
public class Tenant
{
    public Guid Id { get; set; }
    public string StoreName { get; set; } = String.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}