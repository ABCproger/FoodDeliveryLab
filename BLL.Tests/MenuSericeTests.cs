namespace BLL.Tests;

using System.Linq.Expressions;
using DAL.Entities;
using DAL.UnitOfWork;
using DTO.Menu;
using Microsoft.Extensions.Logging;
using Moq;
using Services.Implementation;

public class MenuServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ILogger<MenuService>> _mockLogger;
    private readonly MenuService _menuService;

    public MenuServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockLogger = new Mock<ILogger<MenuService>>();
        _menuService = new MenuService(_mockUnitOfWork.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetMenuByIdAsync_ShouldReturnMenu_WhenMenuExists()
    {
        // Arrange
        var menuId = 1;
        var menuEntity = new Menu { Id = menuId, Name = "Test Menu" };
        var menuItems = new List<MenuItem>
        {
            new MenuItem
            {
                Name = "Item 1",
                Description = "Description 1",
                Price = 10.0M,
                ImageUrl = "url1",
                CategoryId = 1,
                IsAvailable = true,
                MenuId = menuId
            }
        };

        // Setup mocks
        _mockUnitOfWork.Setup(uow => uow.Menus.GetByIdAsync(menuId))
            .ReturnsAsync(menuEntity);

        // Mock FindAsync to return the menu items filtered by the menuId
        _mockUnitOfWork.Setup(uow => uow.MenuItems.FindAsync(It.Is<Expression<Func<MenuItem, bool>>>(expr => true)))
            .ReturnsAsync(menuItems);

        // Act
        var result = await _menuService.GetMenuByIdAsync(menuId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Menu", result.MenuName);
        Assert.Single(result.MenuItems);
        Assert.Equal("Item 1", result.MenuItems.First().MenuItemName);
        Assert.Equal(10.0M, result.MenuItems.First().Price);
    }

    [Fact]
    public async Task GetMenuByIdAsync_ShouldThrowKeyNotFoundException_WhenMenuDoesNotExist()
    {
        // Arrange
        int menuId = 99; // Non-existing menu ID
        _mockUnitOfWork.Setup(uow => uow.Menus.GetByIdAsync(menuId))
            .ReturnsAsync((Menu)null); // Mock returns null, simulating that the menu does not exist

        // Act
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _menuService.GetMenuByIdAsync(menuId));
        // Assert
        Assert.Equal($"Menu with ID {menuId} not found.", exception.Message);
    }

    [Fact]
    public async Task CreateMenuAsync_ShouldCreateMenu_WhenValidRequestIsProvided()
    {
        // Arrange
        var createMenuRequest = new CreateMenuRequestDTO
        {
            MenuName = "Test Menu",
            MenuItems = new List<CreateMenuItemRequestDTO>
            {
                new CreateMenuItemRequestDTO
                {
                    MenuItemName = "Test Item",
                    Description = "Test Description",
                    Price = 10.0M,
                    ImageUrl = "test-url",
                    CategoryId = 1,
                    IsAvailable = true
                }
            }
        };

        var menuEntity = new Menu { Id = 1, Name = createMenuRequest.MenuName };

        // Mock repository behavior
        _mockUnitOfWork.Setup(uow => uow.Menus.CreateAsync(It.IsAny<Menu>()))
            .Callback<Menu>(menu => menu.Id = menuEntity.Id)
            .Returns(Task.CompletedTask);

        _mockUnitOfWork.Setup(uow => uow.MenuItems.CreateAsync(It.IsAny<MenuItem>()))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork.Setup(uow => uow.SaveAsync()).Returns(Task.CompletedTask);

        // Act
        await _menuService.CreateMenuAsync(createMenuRequest);

        // Assert
        _mockUnitOfWork.Verify(
            uow => uow.Menus.CreateAsync(It.Is<Menu>(menu => menu.Name == createMenuRequest.MenuName)), Times.Once);

        _mockUnitOfWork.Verify(uow => uow.MenuItems.CreateAsync(It.Is<MenuItem>(item =>
            item.Name == createMenuRequest.MenuItems[0].MenuItemName &&
            item.Description == createMenuRequest.MenuItems[0].Description &&
            item.Price == createMenuRequest.MenuItems[0].Price &&
            item.ImageUrl == createMenuRequest.MenuItems[0].ImageUrl &&
            item.CategoryId == createMenuRequest.MenuItems[0].CategoryId &&
            item.IsAvailable == createMenuRequest.MenuItems[0].IsAvailable &&
            item.MenuId == menuEntity.Id
        )), Times.Exactly(createMenuRequest.MenuItems.Count));

        _mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateMenuAsync_ShouldNotCreateMenuItems_WhenMenuItemsAreNullOrEmpty()
    {
        // Arrange
        var createMenuRequest = new CreateMenuRequestDTO { MenuName = "Test Menu", MenuItems = null };

        var menuEntity = new Menu { Id = 1, Name = createMenuRequest.MenuName };

        _mockUnitOfWork.Setup(uow => uow.Menus.CreateAsync(It.IsAny<Menu>()))
            .Callback<Menu>(menu => menu.Id = menuEntity.Id)
            .Returns(Task.CompletedTask);

        _mockUnitOfWork.Setup(uow => uow.SaveAsync()).Returns(Task.CompletedTask);

        // Act
        await _menuService.CreateMenuAsync(createMenuRequest);

        // Assert
        _mockUnitOfWork.Verify(
            uow => uow.Menus.CreateAsync(It.Is<Menu>(menu => menu.Name == createMenuRequest.MenuName)), Times.Once);

        _mockUnitOfWork.Verify(uow => uow.MenuItems.CreateAsync(It.IsAny<MenuItem>()), Times.Never);

        _mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateMenuAsync_ShouldThrowArgumentNullException_WhenRequestIsNull()
    {
        // Act
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _menuService.CreateMenuAsync(null));

        // Assert
        Assert.Equal("Value cannot be null. (Parameter 'request')", exception.Message);
    }

}