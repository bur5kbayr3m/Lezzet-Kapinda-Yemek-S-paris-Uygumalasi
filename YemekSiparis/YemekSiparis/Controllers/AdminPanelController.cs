using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace YemekSiparis.WebAPI.Controllers
{
    public class AdminPanelController : Controller
    {
        public IActionResult Login()
        {
            // If already logged in as admin, redirect to dashboard
            if (User.Identity?.IsAuthenticated == true && User.IsInRole("Admin"))
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Dashboard()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Users()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Stores()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Products()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Orders()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Categories()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Logout()
        {
            // Clear the token cookie
            Response.Cookies.Delete("token");
            return RedirectToAction("Login");
        }
    }
}