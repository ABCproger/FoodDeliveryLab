namespace DAL.Repositories.Implementation;

using EF;
using Entities;
using Interfaces;

public class CategoryRepository: BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(BaseDbContext dbContext) : base(dbContext)
    {
    }
}