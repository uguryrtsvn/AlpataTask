using AlpataBLL.Utilities.Security.Jwt;
using AlpataEntities.Dtos.AuthDtos;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Globalization;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using AlpataUI.Helpers.ClientHelper;
using AlpataBLL.BaseResult.Concretes;
using System.Text;
using AlpataUI.Models;
using AlpataUI.Helpers.FileManagerHelper;

namespace AlpataUI.Controllers
{
    public class AccountController : Controller
    {
        readonly IToastNotification _toastNotification;
        private readonly IAlpataClient _alpataClient;
        private readonly IFileManager _fileUploadService;

        public AccountController(IToastNotification toastNotification, IAlpataClient alpataClient, IFileManager fileUploadService)
        {
            _toastNotification = toastNotification;
            _alpataClient = alpataClient;
            _fileUploadService = fileUploadService;
        }
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Dashboard");
            return View();
        }
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginAuthDto, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Dashboard");

            var loginResult = await _alpataClient.Action<Token, LoginDto>("Account/Login", loginAuthDto);
            if (loginResult.Success)
            {
                await SignInUser(loginResult.Data, loginAuthDto.RememberMe);
                 
                if (!string.IsNullOrWhiteSpace(returnUrl))
                {
                    if (!returnUrl.StartsWith('/'))
                        returnUrl = $"/{returnUrl}";

                    return LocalRedirect(returnUrl);
                }
                return RedirectToAction("Index", "Dashboard");
            }
            _toastNotification.AddErrorToastMessage(loginResult.Message);
            ViewBag.returnUrl = returnUrl;
            ViewBag.Message = loginResult.Message;
            return View(loginAuthDto);
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _toastNotification.AddInfoToastMessage("Çıkış yapıldı.");
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto, IFormFile file)
        { 
            var imageResult = await _fileUploadService.SaveFileAsync(new FileDto() { FormFile = file}, FileStorageLocation.Local);
            if (imageResult.Success)
            { 
                registerDto.ImagePath = imageResult.Message;
                var result = await _alpataClient.Add(registerDto, "Account/Register");
                if (result.Success)
                {
                    _toastNotification.AddSuccessToastMessage(result.Message);
                    return View("~/Views/Account/Login.cshtml", new LoginDto() { Email = registerDto.Email });
                }
                else
                {
                    _toastNotification.AddErrorToastMessage(result.Message);
                    await _fileUploadService.DeleteFileAsync(registerDto.ImagePath);
                }
            }
            _toastNotification.AddErrorToastMessage(imageResult.Message);
            return View(registerDto);
        }

        private JwtSecurityToken HandleJwtToken(Token token)
        {
            JwtSecurityToken jwtToken = (new JwtSecurityTokenHandler().ReadToken(token.AccessToken) as JwtSecurityToken)!;
            return jwtToken;
        }

        private async Task SignInUser(Token token, bool rememberMe)
        {
            JwtSecurityToken jwtToken = HandleJwtToken(token);

            var claims = new List<Claim>()
            {
                new("AccessToken", token.AccessToken),
                new("RefreshToken", token.RefreshToken),
                new("TokenExp", token.Expiration.ToLongDateString()),
                new(ClaimTypes.Name, jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name).Value),
                new("Id", jwtToken.Claims.FirstOrDefault(claim => claim.Type == "Id")?.Value),
                new(ClaimTypes.Email, jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email).Value),
                new("ImagePath", jwtToken.Claims.FirstOrDefault(claim => claim.Type =="ImagePath").Value)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties()
            {
                ExpiresUtc = DateTime.UtcNow.AddDays(365),
                IssuedUtc = DateTime.UtcNow,
                IsPersistent = rememberMe,
                AllowRefresh = false
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
    }
}
