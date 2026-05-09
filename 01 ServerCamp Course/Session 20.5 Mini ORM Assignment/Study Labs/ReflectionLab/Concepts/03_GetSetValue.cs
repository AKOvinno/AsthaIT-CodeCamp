// ================================================================
//  03 — GetValue() and SetValue()
//  Once you have a PropertyInfo, you can READ or WRITE
//  that property on a real object — without knowing the
//  property name at compile time.
// ================================================================

using System.Reflection;

namespace ReflectionLab.Concepts;

public static class Concept03_GetSetValue
{
    public static void Run()
    {
        Console.WriteLine("=== 03: GetValue() and SetValue() ===\n");

        var product = new Product { Name = "Keyboard", Price = 89.99m, InStock = true };
        Type type   = typeof(Product);

        // ── Reading with GetValue() ───────────────────────────────
        PropertyInfo nameProp = type.GetProperty("Name")!;
        // The ! tells C#: "trust me, this property exists"

        object? value = nameProp.GetValue(product);
        Console.WriteLine($"Read Name: {value}");        // Keyboard

        // ── Writing with SetValue() ───────────────────────────────
        nameProp.SetValue(product, "Mouse");
        Console.WriteLine($"After SetValue: {product.Name}");  // Mouse

        // ── Loop through all properties and read values ───────────
        // This is exactly what DbSet.AddParams() does when
        // building INSERT parameters.
        Console.WriteLine("\n-- All property values via reflection --");
        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            object? val = prop.GetValue(product);
            Console.WriteLine($"  {prop.Name,-15} = {val ?? "NULL"}");
        }

        // ── Writing back from a fake DB row ──────────────────────
        // This simulates what DbSet.Map() does when reading from DB.
        Console.WriteLine("\n-- Simulating DB row → Product mapping --");
        var freshProduct = new Product();                  // blank object

        type.GetProperty("Id")!.SetValue(freshProduct, 42);
        type.GetProperty("Name")!.SetValue(freshProduct, "Monitor");
        type.GetProperty("Price")!.SetValue(freshProduct, 299.99m);
        type.GetProperty("InStock")!.SetValue(freshProduct, true);

        Console.WriteLine($"  Id={freshProduct.Id}, Name={freshProduct.Name}, Price={freshProduct.Price}");
        Console.WriteLine();
    }
}

// ── Key takeaway ─────────────────────────────────────────────────
// GetValue → used in DbSet.AddParams() to read property values
//            from an entity and pass them as SQL parameters.
// SetValue → used in DbSet.Map() to fill a blank entity object
//            with values read from a NpgsqlDataReader row.
