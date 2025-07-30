using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using YemekSiparis.Application.Dtos;
using YemekSiparis.Application.Interfaces;

namespace YemekSiparis.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [Authorize(Roles = "User")]
        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] CartDto cartDto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _cartService.AddToCartAsync(userId, cartDto);
            return Ok("Ürün sepete eklendi.");
        }

        [Authorize(Roles = "User")]
        [HttpPut("update-item")]
        public async Task<IActionResult> UpdateCartItem([FromBody] UpdateCartItemDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _cartService.UpdateCartItemAsync(userId, dto);
            return Ok("Sepet güncellendi.");
        }

        [Authorize(Roles = "User")]
        [HttpGet("my-cart")]
        public async Task<IActionResult> GetCartItems()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var items = await _cartService.GetCartItemsAsync(userId);
            return Ok(items);
        }

        [Authorize(Roles = "User")]
        [HttpDelete("remove/{id}")]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            await _cartService.RemoveFromCartAsync(id);
            return Ok("Ürün sepetten kaldırıldı.");
        }
    }
}
