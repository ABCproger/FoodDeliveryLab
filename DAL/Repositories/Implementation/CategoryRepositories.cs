namespace DAL.Repositories.Implementation;

using EF;
using Entities;
using Interfaces;

public class CategoryRepositories: BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepositories(BaseDbContext dbContext) : base(dbContext)
    {
    }
}