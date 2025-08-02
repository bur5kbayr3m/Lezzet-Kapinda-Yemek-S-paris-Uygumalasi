using Microsoft.AspNetCore.Mvc;

namespace YemekSiparis.WebAPI.Controllers
{
    public class AdminViewController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}