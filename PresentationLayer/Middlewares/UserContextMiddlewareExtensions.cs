using Microsoft.AspNetCore.Routing;

namespace PresentationLayer.Middlewares
{
    public static class UserContextMiddlewareExtensions
    {
        public static IApplicationBuilder UseUserContext(this IApplicationBuilder builder) => builder.UseMiddleware<UserContextMiddleware>();
    }
}
