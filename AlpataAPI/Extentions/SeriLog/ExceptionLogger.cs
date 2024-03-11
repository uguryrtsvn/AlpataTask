using Serilog.Context;
using Serilog;
using System.Text;
using AlpataBLL.BaseResult.Concretes;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace AlpataAPI.Extentions.SeriLog
{
    public class ExceptionLogger
    {
        public static void LogException(Exception? exception, HttpContext httpContext)
        {
            using (LogContext.PushProperty("UserId", httpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "Id")?.Value))
            {
                if (exception?.GetType() != typeof(ValidationException))
                {
                    string? requestPath = httpContext?.Request?.Path;
                    var requestHeaders = httpContext?.Request?.Headers;

                    StringBuilder stringBuilder = new();
                    stringBuilder.AppendLine($"An unhandled exception occurred at {requestPath}");
                    stringBuilder.AppendLine("Request Headers:");

                    if (requestHeaders != null)
                        foreach (var header in requestHeaders)
                            stringBuilder.AppendLine($"{header.Key}: {header.Value}");

                    while (exception != null)
                    {
                        stringBuilder.AppendLine($"Exception: {exception.GetType().FullName}");
                        stringBuilder.AppendLine($"Message: {exception.Message}");
                        stringBuilder.AppendLine("Stack Trace:");
                        stringBuilder.Append($"{exception.StackTrace}");

                        exception = exception.InnerException;
                    }

                    Log.Error(exception, stringBuilder.ToString());
                }
            }
        }
    }
    public class ExceptionHandlingMiddleware
    {
        private RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e, httpContext);
                await HandleExceptionAsync(httpContext, e);
            }
        }


        private  Task HandleExceptionAsync(HttpContext httpContext, Exception e)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            ErrorDataResult<object?> errorResponse = new(default, "Internal Server Error"); // Apidan Dönen Data default veya null gitmezse ui'dan istek atan AlpataClient response'u deserialize edemiyor

            if (e.GetType() == typeof(UnauthorizedAccessException))
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                errorResponse.Message = e.Message;
            }

            var jsonError = JsonConvert.SerializeObject(errorResponse);

            return httpContext.Response.WriteAsync(jsonError);
        }
    }

    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
