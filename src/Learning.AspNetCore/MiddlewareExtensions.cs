using Microsoft.EntityFrameworkCore;

namespace Learning.AspNetCore
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCurrentUser(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CurrentUserMiddleware>();
        }

        public static IApplicationBuilder UseAutoSaveChange<TDbContext>(this IApplicationBuilder app) where TDbContext : DbContext
        {
            return app.UseMiddleware<EFAutoSaveChangeMiddleware<TDbContext>>();
        }
    }
}
