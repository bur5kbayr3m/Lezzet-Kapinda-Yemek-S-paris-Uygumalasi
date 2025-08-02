using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using YemekSiparis.Application.Dtos;
using YemekSiparis.Application.Interfaces;
using YemekSiparis.Application.ViewModels;

namespace YemekSiparis.WebAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStoreService _storeService;

        public HomeController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        public IActionResult Index()
        {
            // Popüler restoranlar
            var popularStores = _storeService.GetPopularStores() ?? new List<StoreDto>();
            // Tüm restoranlar
            var allStores = _storeService.GetAllStores() ?? new List<StoreDto>();

            // ViewModel’i oluştur ve her iki listeyi ata
            var model = new HomeViewModel
            {
                PopularStores = popularStores,
                AllStores = allStores
            };

            return View(model);
        }

        public IActionResult AdminLogin()
        {
            return View();
        }
    }
}
