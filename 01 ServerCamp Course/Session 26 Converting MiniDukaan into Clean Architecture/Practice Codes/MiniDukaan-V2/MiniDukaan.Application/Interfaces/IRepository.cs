namespace MiniDukaan.Application.Interfaces;
public interface IRepository<T>
{
    public Task<T?> GetByIdAsync(Guid id);

    public Task AddAsync(T entity);
    public Task SaveChangesAsync();
}
