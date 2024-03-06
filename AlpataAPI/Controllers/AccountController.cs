using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlpataAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> Register()
        {
            return Ok();
        }
    }
}
