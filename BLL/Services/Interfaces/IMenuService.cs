namespace BLL.Services.Interfaces;

using DTO.Menu;

public interface IMenuService
{
    Task<GetMenuResponseDTO> GetMenuByIdAsync(int menuId);
}