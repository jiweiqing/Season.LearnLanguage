using IdentityService.Domain;
using System.Security.Claims;

namespace IdentityService.Host
{
    public class CurrentUserMiddleware
    {
        private readonly RequestDelegate _next;
        public CurrentUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context,CurrentUserContext currentUser)
        {
            // 是否认证通过
            if (context.User != null && context.User.Identity != null && context.User.Identity.IsAuthenticated)
            {
                // claims 是否包含NameIdentifier
                Claim? claim = context.User.FindFirst(ClaimTypes.NameIdentifier);
                if (claim != null)
                {
                    currentUser.SetValue(Convert.ToInt64(claim.Value));
                }
            }

            await _next(context);
        }
    }
}
