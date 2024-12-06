namespace DAL.Entities;

public class MenuItem : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    public int CategoryId { get; set; }
    public int MenuId { get; set; }
    public bool IsAvailable { get; set; }
    public Category Category { get; set; }
    public Menu Menu { get; set; }
}