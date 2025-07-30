using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using YemekSiparis.Application.Dtos;
using YemekSiparis.Application.Interfaces;
using YemekSiparis.Application.ViewModels;

namespace YemekSiparis.WebAPI.Controllers
{
    [ApiController]
    [Route("api/store")]
    public class StoreController : Controller
    {
        private readonly IStoreService _storeService;
        private readonly IUserService _userService;
        private readonly IProductService _productService;

        private static readonly string[] DefaultStoreImages = new[]
        {
            "burger-meal.jpg",
            "pizza-meal.jpg",
            "sushi-meal.jpg",
            "taco-meal.jpg",
            "noodle-meal.jpg"
        };

        public StoreController(IStoreService storeService, IUserService userService, IProductService productService)
        {
            _storeService = storeService;
            _userService = userService;
            _productService = productService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] StoreDto storeDto)
        {
            storeDto.ImageUrl ??= "burger-meal.jpg";
            storeDto.BannerImageUrl ??= "burger-meal.jpg";
            storeDto.Phone ??= "Telefon girilmedi";
            storeDto.Address ??= "Adres girilmedi";
            storeDto.Description ??= "Açıklama girilmedi";
            storeDto.WorkingHours ??= "09:00-23:00";
            storeDto.Products ??= new List<ProductDto>();

            await _storeService.CreateStoreAsync(storeDto);
            return Ok("Mağaza başarıyla oluşturuldu.");
        }

        [Authorize(Roles = "Store")]
        [HttpPost("request-username-change")]
        public async Task<IActionResult> RequestUsernameChange([FromBody] RequestUsernameChangeDto dto)
        {
            var storeIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(storeIdClaim, out int storeId))
                return Unauthorized("Store kimliği alınamadı.");

            await _storeService.RequestUsernameChangeAsync(storeId, dto);
            return Ok("Mağaza adı değişiklik isteği gönderildi.");
        }

        [Authorize(Roles = "Store")]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var storeIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(storeIdClaim, out int storeId))
                return Unauthorized("Store kimliği alınamadı.");

            await _storeService.ChangePasswordAsync(storeId, dto);
            return Ok("Şifre başarıyla değiştirildi.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] StoreLoginDto loginDto)
        {
            var token = await _storeService.LoginAsync(loginDto);
            if (string.IsNullOrEmpty(token))
                return BadRequest("E-posta veya şifre hatalı.");

            return Ok(new { Token = token });
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchStores(
            [FromQuery] string? productName,
            [FromQuery] string? category,
            [FromQuery] string? sortBy)
        {
            var stores = await _storeService.SearchStoresAsync(productName, category, sortBy);
            return Ok(stores);
        }

        [Authorize(Roles = "Store")]
        [HttpGet("reviews")]
        public async Task<IActionResult> GetStoreReviews()
        {
            var storeIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(storeIdClaim, out int storeId))
                return Unauthorized("Store kimliği alınamadı.");

            var reviews = await _userService.GetStoreReviewsAsync(storeId);
            return Ok(reviews);
        }

        [Authorize(Roles = "Store")]
        [HttpGet("my-products")]
        public async Task<IActionResult> GetMyProducts()
        {
            var storeIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(storeIdClaim, out int storeId))
                return Unauthorized("Store kimliği alınamadı.");

            var products = await _productService.GetMyProductsAsync(storeId);
            return Ok(products);
        }

        // ---- MAĞAZA SAYFASI SEKME ACTIONLARI ----

        // GENEL SEKME (Detay)
        [HttpGet("~/Store/Detail/{id}")]
        public IActionResult Detail(int id)
        {
            var store = _storeService.GetById(id);
            if (store == null)
                return NotFound();

            var imgIndex = id % DefaultStoreImages.Length;
            var imageUrl = DefaultStoreImages[imgIndex];

            var model = new StoreDetailViewModel
            {
                Id = store.Id,
                Name = store.Name,
                Description = store.Description,
                BannerImageUrl = imageUrl,
                Address = store.Address,
                WorkingHours = store.WorkingHours,
                ContactPhone = store.Phone,
                ContactMail = store.Email
            };

            ViewBag.ActiveTab = "Detail";
            // Views/Store/_RestaurantDetail.cshtml
            return View("_RestaurantDetail", model);
        }

        // MENÜ SEKME
        [HttpGet("~/Store/Menu/{id}")]
        public async Task<IActionResult> Menu(int id)
        {
            var store = _storeService.GetById(id);
            var products = await _productService.GetByStoreId(id);

            var model = new StoreMenuViewModel
            {
                Store = store,
                Products = products
            };

            ViewBag.ActiveTab = "Menu";
            // Views/Store/StoreMenu.cshtml
            return View("StoreMenu", model);
        }

        // KAMPANYALAR SEKME
        [HttpGet("~/Store/Campaigns/{id}")]
        public IActionResult Campaigns(int id)
        {
            ViewBag.ActiveTab = "Campaigns";
            ViewBag.Title = "Kampanyalar";
            // Views/Store/ComingSoon.cshtml
            return View("ComingSoon");
        }

        // DEĞERLENDİRMELER SEKME
        [HttpGet("~/Store/Reviews/{id}")]
        public IActionResult Reviews(int id)
        {
            ViewBag.ActiveTab = "Reviews";
            ViewBag.Title = "Değerlendirmeler";
            // Views/Store/ComingSoon.cshtml
            return View("ComingSoon");
        }

        // ANA SAYFA MAĞAZALAR
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var stores = await _storeService.SearchStoresAsync(
                productName: null,
                category: null,
                sortBy: null
            );
            return View(stores);
        }
    }
}
