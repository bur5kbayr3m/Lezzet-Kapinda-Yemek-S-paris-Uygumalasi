using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using YemekSiparis.Application.Dtos;
using YemekSiparis.Application.Interfaces;

[Authorize(Roles = "Store")]

public class StorePanelController : Controller
{
    private readonly IStoreService _storeService;

    public StorePanelController(IStoreService storeService)
    {
        _storeService = storeService;
    }


    [HttpGet]
    public async Task<IActionResult> Panel()
    {

        var storeIdClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        // Alternatif string: x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"

        if (storeIdClaim == null)
            return Unauthorized();

        int storeId = int.Parse(storeIdClaim);
        var model = await _storeService.GetStorePanelInfoAsync(storeId);

        return View("StorePanel", model);
    }

    [Route("api/test/token")]
    [HttpGet]
    public IActionResult GetTokenFromCookie()
    {
        var cookieToken = Request.Cookies["token"];
        return Ok(cookieToken ?? "cookie yok");
    }
    [HttpGet]
    public async Task<IActionResult> Menu()
    {
        var storeIdClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        if (storeIdClaim == null)
            return Unauthorized();

        int storeId = int.Parse(storeIdClaim);

        var store = await _storeService.GetByIdWithProductsAsync(storeId);

        var model = new StoreWithProductsDto
        {
            Id = store.Id,
            Name = store.Name,
            Description = store.Description,
            Address = store.Address,
            Email = store.Email,
            Phone = store.Phone,
            Products = store.Products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                ImageUrl = p.ImageUrl,
                Category = p.Category
            }).ToList()
        };

        return View("Menu", model);
    }

    public IActionResult AddProduct()
    {
        return View();
    }

}
