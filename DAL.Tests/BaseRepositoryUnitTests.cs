namespace DAL.Tests;

using EF;
using Entities;
using Microsoft.EntityFrameworkCore;
using Moq;

public class BaseRepositoryUnitTests
{
    [Fact]
    public async Task Create_InputMenuInstance_CalledAddMethodOfDBSetWithMenuInstance()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<BaseDbContext>().Options;
        var mockContext = new Mock<BaseDbContext>(options);
        var mockDbSet = new Mock<DbSet<Menu>>();
        mockContext.Setup(context => context.Set<Menu>()).Returns(mockDbSet.Object);
        var repository = new TestMenuRepository(mockContext.Object);
        var expectedMenu = new Mock<Menu>().Object;

        // Act
        await repository.CreateAsync(expectedMenu);
    
        // Assert
        mockDbSet.Verify(dbSet => dbSet.AddAsync(expectedMenu, It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task GetById_InputMenuId_ReturnsMenuInstance()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<BaseDbContext>().Options;
        var mockContext = new Mock<BaseDbContext>(options);
        var mockDbSet = new Mock<DbSet<Menu>>();

        var expectedMenu = new Menu { Id = 1, Name = "Test Menu", MenuItems = new List<MenuItem>() };
    
        mockDbSet.Setup(m => m.FindAsync(1)).ReturnsAsync(expectedMenu);
        mockContext.Setup(context => context.Set<Menu>()).Returns(mockDbSet.Object);

        var repository = new TestMenuRepository(mockContext.Object);

        // Act
        var result = await repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedMenu.Id, result.Id);
        Assert.Equal(expectedMenu.Name, result.Name);
        Assert.Empty(result.MenuItems);
    }
    
    [Fact]
    public async Task Delete_InputMenuId_CallsRemoveMethodOfDbSetWithMenuInstance()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<BaseDbContext>().Options;
        var mockContext = new Mock<BaseDbContext>(options);
        var mockDbSet = new Mock<DbSet<Menu>>();

        var menuToDelete = new Menu { Id = 1, Name = "Menu to Delete", MenuItems = new List<MenuItem>() };

        // Setup FindAsync to return the menu instance
        mockDbSet.Setup(m => m.FindAsync(1)).ReturnsAsync(menuToDelete);
        mockContext.Setup(context => context.Set<Menu>()).Returns(mockDbSet.Object);

        var repository = new TestMenuRepository(mockContext.Object);

        // Act
        await repository.DeleteAsync(1);

        // Assert
        mockDbSet.Verify(dbSet => dbSet.Remove(menuToDelete), Times.Once());
        mockContext.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }
}