using Xunit;
using Moq;

namespace DAL.Tests;

using EF;
using Entities;
using Microsoft.EntityFrameworkCore;

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
        var expectedMeal = new Mock<Menu>().Object;

        // Act
        await repository.CreateAsync(expectedMeal);

        // Assert
        mockDbSet.Verify(dbSet => dbSet.AddAsync(expectedMeal, It.IsAny<CancellationToken>()), Times.Once());
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

}