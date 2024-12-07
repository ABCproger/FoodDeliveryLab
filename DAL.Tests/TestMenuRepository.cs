namespace DAL.Tests;

using EF;
using Entities;
using Repositories.Implementation;

public class TestMenuRepository : BaseRepository<Menu>
{
    public TestMenuRepository(BaseDbContext dbContext) : base(dbContext)
    {
    }
}