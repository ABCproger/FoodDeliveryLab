namespace DAL.Repositories.Interfaces;

using System.Linq.Expressions;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task CreateAsync(T item);
    Task UpdateAsync(T item);
    Task DeleteAsync(int id);
}