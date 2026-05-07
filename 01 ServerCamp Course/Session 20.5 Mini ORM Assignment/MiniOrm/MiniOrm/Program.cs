// ============================================================
//  MiniOrm – Step-by-step demo walkthrough (Task 05)
// ============================================================
using MiniOrm.Data;
using MiniOrm.Models;

// ── Step 1: Entities are defined with attributes (see Models/) ─────────────
Console.WriteLine("=== MiniOrm Demo ===\n");

// ── Step 2 & 3: Create the context; connection string from env var ─────────
var connStr = Environment.GetEnvironmentVariable("MINIORM_CONN")
    ?? throw new InvalidOperationException(
        "Set the MINIORM_CONN environment variable before running the demo.\n" +
        "Example: export MINIORM_CONN=\"Host=localhost;Database=miniorm;Username=postgres;Password=secret\"");

var db = new AppDbContext(connStr);
db.TestConnection();
Console.WriteLine("\n>> Run 'dotnet run -- migrations apply' in MiniOrm.Migrations/ before continuing.\n");

// ── Step 4: Insert ─────────────────────────────────────────────────────────
Console.WriteLine("--- Insert ---");
var keyboard = new Product { Name = "Keyboard", Price = 89.99m, Discount = null, InStock = true };
int keyId = db.Products.Insert(keyboard);

var mouse = new Product { Name = "Mouse", Price = 29.99m, Discount = 2.50m, InStock = true };
int mouseId = db.Products.Insert(mouse);

// ── Step 5: FindById ───────────────────────────────────────────────────────
Console.WriteLine("\n--- FindById ---");
var found = db.Products.FindById(keyId);
Console.WriteLine($"Found → {found?.Name}, Price={found?.Price}, Discount={found?.Discount?.ToString() ?? "NULL"}");

// ── Step 5: Update ─────────────────────────────────────────────────────────
Console.WriteLine("\n--- Update ---");
found!.Price = 79.99m;
found.Discount = 5.00m;
db.Products.Update(found);

var updated = db.Products.FindById(keyId);
Console.WriteLine($"After update → Price={updated?.Price}, Discount={updated?.Discount}");

// ── Step 5: GetAll ─────────────────────────────────────────────────────────
Console.WriteLine("\n--- GetAll ---");
var all = db.Products.GetAll().ToList();
Console.WriteLine($"{all.Count} product(s) in table:");
foreach (var p in all)
    Console.WriteLine($"  Id={p.Id}, Name={p.Name}, Price={p.Price}, Discount={p.Discount?.ToString() ?? "NULL"}");

// ── Step 5: Delete ─────────────────────────────────────────────────────────
Console.WriteLine("\n--- Delete ---");
db.Products.Delete(keyId);
db.Products.Delete(mouseId);
var remaining = db.Products.GetAll().ToList();
Console.WriteLine($"{remaining.Count} product(s) remaining ✓");

Console.WriteLine("\n=== Demo complete ===");

// ── AppDbContext (defined here for assignment clarity) ─────────────────────
public class AppDbContext : DbContext
{
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;

    public AppDbContext(string connStr) : base(connStr) { }
}
