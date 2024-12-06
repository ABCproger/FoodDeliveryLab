namespace DAL.Repositories.Implementation;

using EF;
using Entities;
using Interfaces;
using Microsoft.EntityFrameworkCore;

public class MenuItemRepository : BaseRepository<MenuItem>, IMenuItemRepository
{
    private readonly DbSet<MenuItem> _set;
    public MenuItemRepository(BaseDbContext dbContext) : base(dbContext)
    {
        _set = dbContext.Set<MenuItem>();
    }

    public async Task<List<MenuItem>> GetMenuItemsByCategoryIdAsync(int categoryId)
    {
        return await _set.Where(m => m.CategoryId == categoryId).ToListAsync();
    }

    public async Task<List<MenuItem>> GetAvailableItems()
    {
        return await _set.Where(m => m.IsAvailable == true).ToListAsync();
    }
}