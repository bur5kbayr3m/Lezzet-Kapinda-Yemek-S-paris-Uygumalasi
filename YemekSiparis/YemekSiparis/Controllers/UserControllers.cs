using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using YemekSiparis.Application.Dtos;
using YemekSiparis.Application.Interfaces;

namespace YemekSiparis.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [Authorize]
        [HttpPost("request-username-change")]
        public async Task<IActionResult> RequestUsernameChange([FromBody] RequestUsernameChangeDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _userService.RequestUsernameChangeAsync(userId, dto);
            return Ok("Kullanıcı adı değişiklik isteği gönderildi.");
        }
        [Authorize(Roles = "User")]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _userService.ChangePasswordAsync(userId, dto);
            return Ok("Şifre başarıyla değiştirildi.");
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            await _userService.CreateUserAsync(userDto);
            return Ok("Kullanıcı başarıyla oluşturuldu.");
        }
        [Authorize(Roles = "User")]
        [HttpPut("orders/update/{orderId}")]
        public async Task<IActionResult> UpdateOrder(int orderId, [FromBody] UpdateOrderDto dto)
        {
            await _userService.UpdateOrderByUserAsync(orderId, dto);
            return Ok("Sipariş başarıyla güncellendi.");
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var token = await _userService.LoginAsync(loginDto);
            return Ok(new { Token = token });
        }
        [Authorize(Roles = "User")]
        [HttpPost("add-store-review")]
        public async Task<IActionResult> AddStoreReview([FromBody] AddStoreReviewDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _userService.AddStoreReviewAsync(userId, dto);
            return Ok("Yorum ve puanlama kaydedildi.");
        }

    }
}
