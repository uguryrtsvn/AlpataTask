using AlpataBLL.Services.Abstracts;
using AlpataEntities.Dtos.AuthDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlpataAPI.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]  
    [ApiVersion("1.0")]
    public class AccountController : BaseController
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
