using AlpataEntities.Dtos.AuthDtos;
using AlpataUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.Diagnostics;
using System.Security.Claims;

namespace AlpataUI.Controllers
{
    public class HomeController : Controller
    {
        readonly IToastNotification _toastNotification;

        public HomeController(IToastNotification toastNotification)
        { 
            _toastNotification = toastNotification;
        }
  
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Dashboard");
            return View(new RegisterDto());
        }
        public IActionResult Login() => View();
        
        public IActionResult Loginn() => View();

    }
}
