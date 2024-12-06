namespace DAL.Repositories.Implementation;

using System.Linq.Expressions;
using EF;
using Interfaces;
using Microsoft.EntityFrameworkCore;

public class BaseRepository<T> : IRepository<T> where T: class
{
    private readonly DbSet<T> _set;
    private readonly BaseDbContext _dbContext;

    public BaseRepository(BaseDbContext dbContext)
    {
        _dbContext = dbContext;
        _set = dbContext.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _set.ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        var item = await _set.FindAsync(id);
        
        if (item == null)
        {
            throw new KeyNotFoundException($"Item with Id {id} not found.");
        }

        return item;
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _set
            .Where(predicate)
            .ToListAsync();
    }

    public async Task CreateAsync(T item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));
        
        await _set.AddAsync(item);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(T item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));
    
        _set.Update(item);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var item = await _set.FindAsync(id);
    
        if (item == null)
            throw new KeyNotFoundException($"Entity with ID {id} was not found.");
    
        _set.Remove(item);
        await _dbContext.SaveChangesAsync();
    }
}