using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.Security.Claims;

namespace AlpataUI.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        readonly IToastNotification _toastNotification;

        public DashboardController(IToastNotification toastNotification)
        {
            _toastNotification = toastNotification;
        }
        public IActionResult Index()
        {
            _toastNotification.AddInfoToastMessage($"Hoşgeldin {User.Claims.FirstOrDefault(z => z.Type == ClaimTypes.Name).Value}");
            return View();
        }
    }
}
