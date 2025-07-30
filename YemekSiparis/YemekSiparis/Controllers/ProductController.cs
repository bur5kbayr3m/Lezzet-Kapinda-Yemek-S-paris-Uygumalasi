using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using YemekSiparis.Application.Dtos;
using YemekSiparis.Application.Interfaces;

namespace YemekSiparis.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Store")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }


        [HttpPost]
        public async Task<IActionResult> AddProductAsync([FromBody] CreateProductDto productDto)
        {
            var storeIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(storeIdClaim, out var storeId))
                return Unauthorized(new { success = false, message = "Mağaza kimliği alınamadı!" });

            try
            {
                await _productService.AddProductAsync(storeId, productDto);
                return Ok(new
                {
                    success = true,
                    message = "🎉 Ürün başarıyla eklendi! Menünüzde hemen görüntüleyebilirsiniz."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = $"Ürün eklenirken bir hata oluştu: {ex.Message}"
                });
            }
        }
        [HttpGet("my-products")]
        public async Task<IActionResult> GetMyProducts()
        {
            var storeIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(storeIdClaim, out var storeId))
                return Unauthorized(new { success = false, message = "Mağaza kimliği alınamadı!" });

            var products = await _productService.GetProductsByStoreAsync(storeId);
            return Ok(products);
        }
    }
}
