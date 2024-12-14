namespace BLL.Services.Implementation;

using DAL.Entities;
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
    public async Task CreateMenuAsync(CreateMenuRequestDTO request)
    {
        try
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var menuEntity = new Menu
            {
                Name = request.MenuName
            };

            await _unitOfWork.Menus.CreateAsync(menuEntity);

            if (request.MenuItems != null && request.MenuItems.Any())
            {
                var menuItems = request.MenuItems.Select(item => new MenuItem
                {
                    Name = item.MenuItemName,
                    Description = item.Description,
                    Price = item.Price,
                    ImageUrl = item.ImageUrl,
                    CategoryId = item.CategoryId,
                    IsAvailable = item.IsAvailable,
                    MenuId = menuEntity.Id
                });

                foreach (var menuItem in menuItems)
                {
                    await _unitOfWork.MenuItems.CreateAsync(menuItem);
                }
            }

            await _unitOfWork.SaveAsync();

            _logger.LogInformation($"Menu '{menuEntity.Name}' successfully created with ID {menuEntity.Id}.");
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error occurred while creating a new menu.");
            throw;
        }
    }
}