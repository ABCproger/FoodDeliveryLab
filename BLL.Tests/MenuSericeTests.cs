namespace BLL.Tests;

using System.Linq.Expressions;
using DAL.Entities;
using DAL.UnitOfWork;
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
                .ReturnsAsync((Menu)null);  // Mock returns null, simulating that the menu does not exist

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _menuService.GetMenuByIdAsync(menuId));
            
            Assert.Equal($"Menu with ID {menuId} not found.", exception.Message);
        }
    }