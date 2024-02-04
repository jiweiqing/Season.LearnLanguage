using IdentityService.Infrastructure;
using System;

namespace IdentityService.Host
{
    /// <summary>
    /// auto save
    /// </summary>
    public class EFAutoSaveChangeMiddleware
    {
        private readonly RequestDelegate _next;
        public EFAutoSaveChangeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IdentityServiceDbContext dbContext)
        {
            await _next(context);

            if (dbContext.ChangeTracker.HasChanges())
            {
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
