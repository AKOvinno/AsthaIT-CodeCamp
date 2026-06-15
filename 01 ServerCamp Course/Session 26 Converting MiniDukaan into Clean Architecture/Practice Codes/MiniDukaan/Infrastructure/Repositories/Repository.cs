using Microsoft.EntityFrameworkCore;
using MiniDukaan.Infrastructure.Data.DbContext;
public class Repository<T>(ApplicationDbContext dbContext) where T : class
{
    protected readonly ApplicationDbContext _dbContext = dbContext;
    protected readonly DbSet<T> _dbSet = dbContext.Set<T>();
    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Set<T>().FindAsync(id);
    }
    public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
    public async Task SaveChangesAsync() => await _dbContext.SaveChangesAsync();
}