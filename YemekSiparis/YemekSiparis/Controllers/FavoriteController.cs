using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YemekSiparis.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class FavoriteController : ControllerBase
{
    private readonly IFavoriteService _favoriteService;

    public FavoriteController(IFavoriteService favoriteService)
    {
        _favoriteService = favoriteService;
    }

    [Authorize(Roles = "User")]
    [HttpPost("{storeId}")]
    public async Task<IActionResult> AddFavorite(int storeId)
    {
        var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (claim == null) return Unauthorized("Geçersiz token.");
        var userId = int.Parse(claim);

        await _favoriteService.AddFavoriteAsync(userId, storeId);
        return Ok("Favorilere eklendi.");
    }

    [Authorize(Roles = "User")]
    [HttpDelete("{storeId}")]
    public async Task<IActionResult> RemoveFavorite(int storeId)
    {
        var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (claim == null) return Unauthorized("Geçersiz token.");
        var userId = int.Parse(claim);

        await _favoriteService.RemoveFavoriteAsync(userId, storeId);
        return Ok("Favoriden çıkarıldı.");
    }

    [Authorize(Roles = "User")]
    [HttpGet]
    public async Task<IActionResult> GetFavorites()
    {
        var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (claim == null) return Unauthorized("Geçersiz token.");
        var userId = int.Parse(claim);

        var stores = await _favoriteService.GetFavoritesAsync(userId);
        return Ok(stores);
    }
}
