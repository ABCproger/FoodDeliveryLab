namespace BLL.Services.Implementation;

using DAL.UnitOfWork;
using DTO.Menu;
using Interfaces;
using Microsoft.Extensions.Logging;

public class MenuService : IMenuService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<MenuService> _logger;

    public MenuService(
        IUnitOfWork unitOfWork,
        ILogger<MenuService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task<GetMenuResponseDTO> GetMenuByIdAsync(int menuId)
    {
        try
        {
            var menuEntity = await _unitOfWork.Menus.GetByIdAsync(menuId);

            if (menuEntity == null)
            {
                _logger.LogWarning($"Menu with ID {menuId} not found.");
                throw new KeyNotFoundException($"Menu with ID {menuId} not found.");
            }
            
            var menuItems = await _unitOfWork.MenuItems.FindAsync(item => item.MenuId == menuId);
            
            var response = new GetMenuResponseDTO
            {
                MenuName = menuEntity.Name,
                MenuItems = menuItems.Select(item => new MenuItemResponseDTO
                {
                    MenuItemName = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    ImageUrl = item.ImageUrl,
                    CategoryId = item.CategoryId,
                    IsAvailable = item.IsAvailable
                }).ToList()
            };

            return response;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error occurred while fetching menu with ID {menuId}.");
            throw;
        }
    }
}