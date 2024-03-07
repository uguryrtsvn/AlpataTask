using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Security.Claims;
using AlpataUI.ClientHelper;

namespace AlpataUI.Infrastructure.Extensions
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CustomAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly string[] _roles;
        public CustomAuthorizeAttribute(string roles = null)
        {
            _roles = roles?.Split(',');
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context.ActionDescriptor.EndpointMetadata.OfType<AnonymousAttribute>().Any()) return;

            string accessToken = string.Empty;
            var result = false;
            accessToken = context.HttpContext.User.FindFirst("AccessToken")?.Value;

            if (accessToken != null)
                result = true;

            if (result)
            { 

                if (_roles is not null)
                {
                    result = false;
                    var roleClaims = context.HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.Role)?.Value.Split(',');

                    if (roleClaims == null)
                    {
                        context.Result = new RedirectResult("~/AccessDenied");
                        return;
                    }

                    foreach (var role in _roles)
                    {
                        if (roleClaims.Contains(role.Trim()))
                        {
                            result = true;
                            break;
                        }
                    }

                    if (!result) context.Result = new RedirectResult("~/AccessDenied");
                }
            }
            else
                context.Result = new RedirectResult($"~/Account/Login?returnUrl={UrlEncoder.Default.Encode(string.IsNullOrEmpty(context.HttpContext.Request.Path) ? "" : context.HttpContext.Request.Path + context.HttpContext.Request.QueryString)}"); 
        }
         
    }
}
