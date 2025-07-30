using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using YemekSiparis.Application.Dtos;
using YemekSiparis.Application.Interfaces;
using YemekSiparis.Infrastructure.Context;

namespace YemekSiparis.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IOrderService _orderService;
        private readonly YemekSiparisDbContext _context;

        public AdminController(IAdminService adminService, IOrderService orderService, YemekSiparisDbContext context)
        {
            _adminService = adminService;
            _orderService = orderService;
            _context = context;
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("stores/{storeId}")]
        public async Task<IActionResult> DeleteStore(int storeId)
        {
            await _adminService.DeleteStoreAsync(storeId);
            return Ok("Mağaza başarıyla silindi.");
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AdminRegisterDto dto)
        {
            await _adminService.RegisterAsync(dto);
            return Ok("Admin başarıyla kaydedildi.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AdminLoginDto dto)
        {
            var token = await _adminService.LoginAsync(dto);
            return Ok(new { Token = token });
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("stores/{storeId}")]
        public async Task<IActionResult> UpdateStore(int storeId, [FromBody] UpdateStoreDto dto)
        {
            await _adminService.UpdateStoreAsync(storeId, dto);
            return Ok("Mağaza bilgileri güncellendi.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("users/{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UpdateUserDto dto)
        {
            await _adminService.UpdateUserAsync(userId, dto);
            return Ok("Kullanıcı bilgileri başarıyla güncellendi.");
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("orders/{orderId}")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderDto dto)
        {
            await _adminService.UpdateOrderStatusAsync(orderId, dto);
            return Ok("Sipariş durumu başarıyla güncellendi.");
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("users/{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            await _adminService.DeleteUserAsync(userId);
            return Ok("Kullanıcı başarıyla silindi.");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("orders/{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            await _orderService.DeleteOrderAsync(orderId);
            return Ok("Sipariş başarıyla silindi.");
        }
        [HttpGet("username-change-requests")]
        public async Task<IActionResult> GetPendingUsernameChangeRequests()
        {
            var requests = await _context.UsernameChangeRequests
                .Where(r => r.Status == "Pending")
                .ToListAsync();

            return Ok(requests);
        }

        [HttpPost("approve-username-change/{requestId}")]
        public async Task<IActionResult> ApproveUsernameChange(int requestId, [FromQuery] bool approve)
        {
            var request = await _context.UsernameChangeRequests.FindAsync(requestId);

            if (request == null || request.Status != "Pending")
                return NotFound("Talep bulunamadı veya zaten işlenmiş.");

            if (approve)
            {
                if (request.UserType == "user")
                {
                    var user = await _context.Users.FindAsync(request.UserId);
                    if (user == null) return NotFound("Kullanıcı bulunamadı.");

                    user.Username = request.NewUsername;
                }
                else if (request.UserType == "store")
                {
                    var store = await _context.Stores.FindAsync(request.UserId);
                    if (store == null) return NotFound("Mağaza bulunamadı.");

                    store.Name = request.NewUsername;
                }

                request.Status = "Approved";
            }
            else
            {
                request.Status = "Rejected";
            }

            await _context.SaveChangesAsync();
            return Ok("İşlem başarıyla tamamlandı.");
        }
        [HttpGet("store-reviews")]
        public async Task<IActionResult> GetAllStoreReviews()
        {
            var reviews = await _context.StoreReviews
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return Ok(reviews);
        }
        [HttpDelete("store-review/{id}")]
        public async Task<IActionResult> DeleteStoreReview(int id)
        {
            var review = await _context.StoreReviews.FindAsync(id);
            if (review == null)
                return NotFound("Yorum bulunamadı.");

            _context.StoreReviews.Remove(review);
            await _context.SaveChangesAsync();

            return Ok("Yorum silindi.");
        }
        [HttpGet("order-reports")]
        public async Task<IActionResult> GetOrderReports([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var reports = await _adminService.GetOrderReportsAsync(startDate, endDate);
            return Ok(reports);
        }
        [HttpGet("all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _adminService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("all-stores")]
        public async Task<IActionResult> GetAllStores()
        {
            var stores = await _adminService.GetAllStoresAsync();
            return Ok(stores);
        }
        [HttpGet("search-user")]
        public async Task<IActionResult> SearchUser([FromQuery] string username)
        {
            var user = await _adminService.GetUserByUsernameAsync(username);
            return Ok(user);
        }

        [HttpGet("search-store")]
        public async Task<IActionResult> SearchStore([FromQuery] string storeName)
        {
            var store = await _adminService.GetStoreByNameAsync(storeName);
            return Ok(store);
        }




    }
}
