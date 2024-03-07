﻿using AlpataBLL.Utilities.Security.Jwt;
using AlpataEntities.Dtos.AuthDtos;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AlpataUI.ClientHelper;
using System.Globalization;
using Microsoft.JSInterop;
using Newtonsoft.Json; 

namespace AlpataUI.Controllers
{
    public class AccountController : Controller
    {
        readonly IToastNotification _toastNotification;
        private readonly IAlpataClient _alpataClient;

        public AccountController(IToastNotification toastNotification, IAlpataClient alpataClient)
        {
            _toastNotification = toastNotification;
            _alpataClient = alpataClient;
        }
 
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginAuthDto, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Dashboard");
             
            var loginResult = await _alpataClient.Action<Token, LoginDto>("Account/Login", loginAuthDto);
            if (loginResult.Success)
            {  
                await SignInUser(loginResult.Data, loginAuthDto.RememberMe);

                JwtSecurityToken token = HandleJwtToken(loginResult.Data);

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
            return View("~/Views/Home/Login.cshtml", loginAuthDto);
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        { 
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _toastNotification.AddInfoToastMessage("Çıkış yapıldı.");
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto,IFormFile file)
        {

            if ((registerDto.Email.ToUpper(new CultureInfo("en-US")) == registerDto.EmailConfirm.ToUpper(new CultureInfo("en-US"))) && registerDto.Password == registerDto.PasswordConfirm)
            {
                var result = await _alpataClient.Add(registerDto, "Account/Register");
                if (result.Success)
                {
                    _toastNotification.AddSuccessToastMessage(result.Message); 
                    return View("~/Views/Home/Login.cshtml", new LoginDto() { Email = registerDto.Email });
                }
                _toastNotification.AddErrorToastMessage(result.Message);
                return View("~/Views/Home/Index.cshtml",registerDto);
            }
            _toastNotification.AddErrorToastMessage("Bilgileri eşleşmiyor."); 
            return View("~/Views/Home/Index.cshtml", registerDto);
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
                new(ClaimTypes.Email, jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email).Value) 
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