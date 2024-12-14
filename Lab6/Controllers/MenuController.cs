namespace Lab6.Controllers;

using BLL.DTO.Menu;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{
    private readonly IMenuService _menuService;
    private readonly ILogger<MenuController> _logger;

    public MenuController(IMenuService menuService, ILogger<MenuController> logger)
    {
        _menuService = menuService;
        _logger = logger;
    }

    [HttpGet("{menuId}")]
    public async Task<IActionResult> GetMenuById(int menuId)
    {
        try
        {
            var result = await _menuService.GetMenuByIdAsync(menuId);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex.Message);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred.");
            return StatusCode(500, new { message = "An error occurred while processing your request." });
        }
    }
    [HttpPost]
    public async Task<IActionResult> CreateMenu([FromBody] CreateMenuRequestDTO request)
    {
        try
        {
            await _menuService.CreateMenuAsync(request);
            return Ok("Menu successfully created");
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogWarning(ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred.");
            return StatusCode(500, new { message = "An error occurred while creating the menu." });
        }
    }
}