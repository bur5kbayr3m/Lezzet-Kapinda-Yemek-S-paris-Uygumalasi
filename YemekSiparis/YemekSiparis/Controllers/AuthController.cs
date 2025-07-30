using Microsoft.AspNetCore.Mvc;
using YemekSiparis.Application.Dtos;
using YemekSiparis.Application.Interfaces;

namespace YemekSiparis.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IStoreService _storeService;

        public AuthController(IUserService userService, IStoreService storeService)
        {
            _userService = userService;
            _storeService = storeService;
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            if (dto.UserType == "user")
                await _userService.ForgotPasswordAsync(dto);
            else if (dto.UserType == "store")
                await _storeService.ForgotPasswordAsync(dto);
            else
                return BadRequest("Geçersiz kullanıcı tipi.");

            return Ok("Şifre sıfırlama işlemi başarılı.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            if (dto.UserType == "user")
                await _userService.ResetPasswordAsync(dto);
            else if (dto.UserType == "store")
                await _storeService.ResetPasswordAsync(dto);
            else
                return BadRequest("Geçersiz kullanıcı tipi.");

            return Ok("Şifre güncellendi.");
        }

    }
}
