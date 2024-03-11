using System.Net;

namespace AlpataUI.Extensions
{
    public class NotFoundRedirectMiddleware
    {
        private readonly RequestDelegate _next; 
        public NotFoundRedirectMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == (int)HttpStatusCode.NotFound)
            {
                context.Response.Redirect("Home/PageNotFound");
            }
            return;
        }
    }
    public static class NotFoundRedirectMiddlewareExtensions
    {
        public static IApplicationBuilder UseNotFoundRedirect(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<NotFoundRedirectMiddleware>();
        }
    }
}
