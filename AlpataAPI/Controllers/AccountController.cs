using AlpataBLL.Services.Abstracts;
using AlpataEntities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlpataAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    { 
        private readonly IAuthenticationService _authenticationService; 

        public AccountController(IAuthenticationService authenticationService)
        { 
            _authenticationService = authenticationService; 
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authenticationService.Login(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _authenticationService.Register(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet, Authorize]
        public IActionResult Auth()
        {
            return Ok();
        }
    }
}
