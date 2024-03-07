using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AlpataAPI.Controllers.V1
{

    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        public string userId => new(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value);
        public Guid parsedUserId => Guid.Parse(userId);
        public string userMail => new(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value);
    }
}
