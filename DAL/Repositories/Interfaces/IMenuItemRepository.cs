namespace DAL.Repositories.Interfaces;

using Entities;

public interface IMenuItemRepository : IRepository<MenuItem>
{
    Task<List<MenuItem>> GetMenuItemsByCategoryIdAsync(int categoryId);
    Task<List<MenuItem>> GetAvailableItems();
}