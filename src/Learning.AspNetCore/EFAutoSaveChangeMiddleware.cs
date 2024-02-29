using Microsoft.EntityFrameworkCore;

namespace Learning.AspNetCore
{
    /// <summary>
    /// auto save
    /// </summary>
    public class EFAutoSaveChangeMiddleware<TDbContext> where TDbContext : DbContext
    {
        private readonly RequestDelegate _next;
        public EFAutoSaveChangeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, TDbContext dbContext)
        {
            await _next(context);

            if (dbContext.ChangeTracker.HasChanges())
            {
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
