// ================================================================
//  01 — typeof and GetType()
//  These are the two entry points into reflection.
//  Everything starts from a Type object.
// ================================================================

namespace ReflectionLab.Concepts;

public static class Concept01_TypeOf
{
    public static void Run()
    {
        Console.WriteLine("=== 01: typeof and GetType() ===\n");

        // typeof — used when you KNOW the type at compile time
        Type type1 = typeof(Product);

        // GetType() — used on a live object when the type is known at runtime
        var obj   = new Product();
        Type type2 = obj.GetType();

        // Both give you the same Type object
        Console.WriteLine($"type1.Name     : {type1.Name}");        // Product
        Console.WriteLine($"type1.FullName  : {type1.FullName}");   // ReflectionLab.Product
        Console.WriteLine($"type1.IsClass   : {type1.IsClass}");    // True
        Console.WriteLine($"type1.IsAbstract: {type1.IsAbstract}"); // False
        Console.WriteLine($"type1 == type2  : {type1 == type2}");   // True — same type either way

        Console.WriteLine();
    }
}

// ── Key takeaway ─────────────────────────────────────────────────
// Type is a blueprint of a class. It holds every piece of
// information about that class — its properties, methods,
// attributes, whether it is abstract, generic, etc.
// In MiniOrm, typeof(T) inside EntityMetadata<T> is how we get
// the blueprint of Product or Order at runtime.

