namespace ReflectionLab.Concepts;

public class AppDbContext
{
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Order>   Orders   { get; set; } = null!;
    // Adding this back in case you're using it in Concept 05
    public string         Name     { get; set; } = "MyApp"; 
}