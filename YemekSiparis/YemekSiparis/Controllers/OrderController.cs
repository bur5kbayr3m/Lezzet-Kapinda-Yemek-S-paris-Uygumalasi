using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using YemekSiparis.Application.Dtos;
using YemekSiparis.Application.Interfaces;

namespace YemekSiparis.WebAPI.Controllers
{
    [Authorize(Roles = "Store")]
    [Route("StorePanel/[controller]")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderController(IOrderService orderService, IHttpContextAccessor httpContextAccessor)
        {
            _orderService = orderService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var storeIdStr = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(storeIdStr, out int storeId))
                return Unauthorized();

            var orders = await _orderService.GetOrdersByStoreIdAsync(storeId);
            return View("Order", orders);
        }

        [HttpPost("update-status")]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            try
            {
                await _orderService.UpdateOrderStatusAsync(id, status);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var storeIdStr = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(storeIdStr, out int storeId))
                return Unauthorized();

            await _orderService.CreateOrderAsync(storeId, dto);
            return Ok("Sipariş oluşturuldu.");
        }
        [HttpGet("detail/{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            var detail = await _orderService.GetOrderDetailAsync(id);
            if (detail == null)
                return NotFound();

            return View("OrderDetail", detail); 
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _orderService.DeleteOrderAsync(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("my-orders-json")]
        public async Task<IActionResult> GetMyOrders()
        {
            var storeIdStr = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(storeIdStr, out int storeId))
                return Unauthorized();

            var orders = await _orderService.GetOrdersByStoreIdAsync(storeId);
            return Ok(orders);
        }
    }
}
