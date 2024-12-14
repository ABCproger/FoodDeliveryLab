namespace BLL.DTO.Menu;

public class CreateMenuRequestDTO
{
    public string MenuName { get; set; }
    public List<CreateMenuItemRequestDTO> MenuItems { get; set; }
}