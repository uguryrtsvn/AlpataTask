using Microsoft.AspNetCore.Authorization;

namespace AlpataUI.Infrastructure.Extensions
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AnonymousAttribute : Attribute, IAllowAnonymous
    {
    }
}
