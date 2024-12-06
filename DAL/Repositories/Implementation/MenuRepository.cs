namespace DAL.Repositories.Implementation;

using EF;
using Entities;
using Interfaces;

public class MenuRepository : BaseRepository<Menu>, IMenuRepository
{
    public MenuRepository(BaseDbContext dbContext) : base(dbContext)
    {
    }
}