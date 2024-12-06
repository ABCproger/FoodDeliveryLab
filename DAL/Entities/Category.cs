namespace DAL.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; }
    public List<MenuItem> MenuItems { get; set; }
}