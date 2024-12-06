namespace DAL.EF;

using Entities;
using Repositories.Implementation;
using Repositories.Interfaces;
using UnitOfWork;

public class EFUnitOfWork : IUnitOfWork
{
    private readonly BaseDbContext _dbContext;
    private MenuRepository _menuRepository;
    private MenuItemRepository _mealRepository;
    private CategoryRepository _menuMealRepository;

    public EFUnitOfWork(BaseDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IMenuRepository Menus
    {
        get
        {
            if (_menuRepository == null)
            {
                _menuRepository = new MenuRepository(_dbContext);
            }
            return _menuRepository;
        }
    }
    

    public IMenuItemRepository MenuItems
    {
        get
        {
            if (_mealRepository == null)
            {
                _mealRepository = new MenuItemRepository(_dbContext);
            }
            return _mealRepository;
        }
    }

    public ICategoryRepository Categories
    {
        get
        {
            if (_menuMealRepository == null)
            {
                _menuMealRepository = new CategoryRepository(_dbContext);
            }
            return _menuMealRepository;
        }
    }

    public async Task SaveAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    private bool _disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}