namespace DAL.EF;

using Entities;
using Microsoft.EntityFrameworkCore;

public class BaseDbContext : DbContext
{
    public DbSet<Menu> Menus { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<Category> Categories { get; set; }

    public BaseDbContext(DbContextOptions options):base(options)
    {
        
    }
}