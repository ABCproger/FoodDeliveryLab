namespace DAL.Tests;

using EF;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Implementation;

public class MenuItemRepositoryTests
{
    private readonly DbContextOptions<BaseDbContext> _dbContextOptions;

    public MenuItemRepositoryTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<BaseDbContext>()
            .UseInMemoryDatabase(databaseName: "TestMenuItemsDb")
            .Options;
    }

    [Fact]
    public async Task GetMenuItemsByCategoryIdAsync_ReturnsCorrectItems()
    {
        // Arrange
        var categoryId = 1;

        await using (var context = new BaseDbContext(_dbContextOptions))
        {
            context.MenuItems.RemoveRange(context.MenuItems);
            await context.SaveChangesAsync();
            
            context.MenuItems.AddRange(
                new MenuItem { Id = 1, CategoryId = 1, IsAvailable = true, Name = "Item 1", Description = "Description 1", ImageUrl = "http://example.com/image1.jpg" },
                new MenuItem { Id = 2, CategoryId = 2, IsAvailable = false, Name = "Item 2", Description = "Description 2", ImageUrl = "http://example.com/image2.jpg" },
                new MenuItem { Id = 3, CategoryId = 1, IsAvailable = true, Name = "Item 3", Description = "Description 3", ImageUrl = "http://example.com/image3.jpg" }
            );
            await context.SaveChangesAsync();
        }

        await using (var context = new BaseDbContext(_dbContextOptions))
        {
            var repository = new MenuItemRepository(context);

            // Act
            var result = await repository.GetMenuItemsByCategoryIdAsync(categoryId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, item => Assert.Equal(categoryId, item.CategoryId));
        }
    }

    [Fact]
    public async Task GetAvailableItems_ReturnsOnlyAvailableItems()
    {
        // Arrange
        await using (var context = new BaseDbContext(_dbContextOptions))
        {
            context.MenuItems.RemoveRange(context.MenuItems);
            await context.SaveChangesAsync();
            
            context.MenuItems.AddRange(
                new MenuItem { Id = 1, CategoryId = 1, IsAvailable = true, Name = "Item 1", Description = "Description 1", ImageUrl = "http://example.com/image1.jpg" },
                new MenuItem { Id = 2, CategoryId = 2, IsAvailable = false, Name = "Item 2", Description = "Description 2", ImageUrl = "http://example.com/image2.jpg" },
                new MenuItem { Id = 3, CategoryId = 1, IsAvailable = true, Name = "Item 3", Description = "Description 3", ImageUrl = "http://example.com/image3.jpg" }
            );
            await context.SaveChangesAsync();
        }

        await using (var context = new BaseDbContext(_dbContextOptions))
        {
            var repository = new MenuItemRepository(context);

            // Act
            var result = await repository.GetAvailableItems();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, item => Assert.True(item.IsAvailable));
        }
    }
}
