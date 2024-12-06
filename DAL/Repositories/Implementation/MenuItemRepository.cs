namespace DAL.Repositories.Implementation;

using EF;
using Entities;
using Interfaces;

public class MenuItemRepository : BaseRepository<MenuItem>, IMenuItemRepository
{
    public MenuItemRepository(BaseDbContext dbContext) : base(dbContext)
    {
    }
}