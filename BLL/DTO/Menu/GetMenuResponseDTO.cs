namespace BLL.DTO.Menu;

public class GetMenuResponseDTO
{
    public string MenuName { get; set; }
    public List<MenuItemResponseDTO> MenuItems { get; set; }
}