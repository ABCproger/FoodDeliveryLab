namespace DAL.UnitOfWork;

using Repositories.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IMenuRepository Menus { get; }
    ICategoryRepository Categories { get; }
    IMenuItemRepository MenuItems { get; }
    Task SaveAsync();
}