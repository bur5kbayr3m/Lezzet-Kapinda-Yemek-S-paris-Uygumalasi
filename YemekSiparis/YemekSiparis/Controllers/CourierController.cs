using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using YemekSiparis.Application.Dtos;
using YemekSiparis.Application.Interfaces;

namespace YemekSiparis.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourierController : ControllerBase
    {
        private readonly ICourierService _courierService;

        public CourierController(ICourierService courierService)
        {
            _courierService = courierService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllCouriers()
        {
            var couriers = await _courierService.GetAllCouriersAsync();
            return Ok(couriers);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("assign")]
        public async Task<IActionResult> AssignCourier([FromBody] AssignCourierDto dto)
        {
            await _courierService.AssignCourierAsync(dto);
            return Ok("Kurye atandı.");
        }

        [Authorize(Roles = "Courier")]
        [HttpGet("orders")]
        public async Task<IActionResult> GetCourierOrders()
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (claim == null) return Unauthorized("Geçersiz token.");

            var courierId = int.Parse(claim);
            var orders = await _courierService.GetOrdersForCourierAsync(courierId);
            return Ok(orders);
        }
    }
}
