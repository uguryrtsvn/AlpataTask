using Microsoft.AspNetCore.Mvc;

namespace AlpataUI.Controllers
{
    public class HomeController : Controller
    {
        [Route("PageNotFound")]

        public IActionResult PageNotFound() => View();
    }
}
