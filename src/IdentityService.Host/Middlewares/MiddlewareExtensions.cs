using Microsoft.AspNetCore.Builder;

namespace IdentityService.Host
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCurrentUser(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CurrentUserMiddleware>();
        }

        public static IApplicationBuilder UseAutoSaveChange(this IApplicationBuilder app)
        {
            return app.UseMiddleware<EFAutoSaveChangeMiddleware>();
        }
    }
}
