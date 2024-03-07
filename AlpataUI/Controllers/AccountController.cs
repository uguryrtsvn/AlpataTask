using Microsoft.AspNetCore.Mvc;

namespace AlpataUI.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
