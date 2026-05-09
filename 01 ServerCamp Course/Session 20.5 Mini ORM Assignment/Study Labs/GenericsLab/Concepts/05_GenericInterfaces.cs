// ================================================================
//  05 — Generic Interfaces
//
//  Interfaces can be generic too.
//  They define a CONTRACT — "any class implementing this
//  must provide these methods for type T".
//
//  MiniOrm doesn't use generic interfaces directly, but
//  understanding them helps you see why DbSet<T> is designed
//  the way it is.
// ================================================================

using GenericsLab.Models;

namespace GenericsLab.Concepts;

public static class Concept05_GenericInterfaces
{
    public static void Run()
    {
        Console.WriteLine("=== 05: Generic Interfaces ===\n");

        // ── Using a generic interface ─────────────────────────────
        // IRepository<T> defines the contract.
        // InMemoryRepository<T> implements it for any T.

        IRepository<Product> productRepo = new InMemoryRepository<Product>();
        productRepo.Add(new Product { Id = 1, Name = "Keyboard", Price = 89.99m });
        productRepo.Add(new Product { Id = 2, Name = "Mouse",    Price = 29.99m });

        Console.WriteLine("-- All Products --");
        foreach (var p in productRepo.GetAll())
            Console.WriteLine($"  {p}");

        var found = productRepo.FindById(1);
        Console.WriteLine($"FindById(1): {found}");

        productRepo.Delete(1);
        Console.WriteLine($"After Delete(1): {productRepo.GetAll().Count()} items left");

        Console.WriteLine();

        // ── Same interface, different T ───────────────────────────
        IRepository<Order> orderRepo = new InMemoryRepository<Order>();
        orderRepo.Add(new Order { Id = 1, Item = "Monitor", Quantity = 2 });
        orderRepo.Add(new Order { Id = 2, Item = "Desk",    Quantity = 1 });

        Console.WriteLine("-- All Orders --");
        foreach (var o in orderRepo.GetAll())
            Console.WriteLine($"  {o}");

        Console.WriteLine();

        // ── Why this matters for MiniOrm ──────────────────────────
        // DbSet<T> is essentially an implementation of IRepository<T>
        // but backed by PostgreSQL instead of a List.
        // The methods are the same:
        //   InsertAsync  → Add
        //   GetAllAsync  → GetAll
        //   FindByIdAsync → FindById
        //   DeleteAsync  → Delete

        Console.WriteLine("-- MiniOrm connection --");
        Console.WriteLine("DbSet<T> implements the same idea as IRepository<T>");
        Console.WriteLine("but uses NpgsqlCommand instead of a List<T>");
        Console.WriteLine();
    }
}

// ── Generic interface — defines the contract ──────────────────────
public interface IRepository<T>
{
    void Add(T item);
    IEnumerable<T> GetAll();
    T? FindById(int id);
    void Delete(int id);
}

// ── Generic implementation — works for any T ─────────────────────
public class InMemoryRepository<T> : IRepository<T> where T : class
{
    private readonly List<T> _items = new();

    public void Add(T item) => _items.Add(item);

    public IEnumerable<T> GetAll() => _items;

    public T? FindById(int id)
    {
        var idProp = typeof(T).GetProperty("Id");
        return _items.FirstOrDefault(item =>
            (int?)idProp?.GetValue(item) == id);
    }

    public void Delete(int id)
    {
        var item = FindById(id);
        if (item != null) _items.Remove(item);
    }
}

// ── Key takeaway ─────────────────────────────────────────────────
// A generic interface defines what methods must exist for T.
// Classes that implement it must provide those methods.
// This lets you swap implementations (in-memory vs PostgreSQL)
// without changing the code that uses the repository.
