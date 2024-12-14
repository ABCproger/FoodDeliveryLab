namespace BLL.DTO.Menu;

public class CreateMenuItemRequestDTO
{
    public string MenuItemName { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    public int CategoryId { get; set; }
    public bool IsAvailable { get; set; }
}