using Learning.Domain;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Learning.AspNetCore
{
    public class CurrentUserMiddleware
    {
        private readonly RequestDelegate _next;
        public CurrentUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, CurrentUserContext currentUser)
        {
            // 是否认证通过
            if (context.User != null && context.User.Identity != null && context.User.Identity.IsAuthenticated)
            {
                #region jwt 撤回待解决

                //// claims 是否包含NameIdentifier
                //Claim? claim = context.User.FindFirst(ClaimTypes.NameIdentifier);
                //Claim? versionClaim = context.User.FindFirst(IdentityContants.Version);
                //if (claim != null && versionClaim != null)
                //{
                //    long userId = Convert.ToInt64(claim.Value);
                //    int version = int.Parse(versionClaim.Value);
                //    var userExists = await dbContext.Users.Where(u => u.Id == userId && u.JwtVersion == version).AnyAsync();
                //    if (!userExists)
                //    {
                //        // 校验不通过直接返回401 TODO:可能需要考虑一下多个类型客户端的情况
                //        // TODO:待优化,可以考虑放在缓存中
                //        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                //        context.Response.ContentType = "application/json; charset=utf-8";

                //        context.Response.ContentLength = 0;

                //        return ;
                //    }
                //    currentUser.SetValue(userId);
                //}

                #endregion

                // claims 是否包含NameIdentifier
                Claim? claim = context.User.FindFirst(ClaimTypes.NameIdentifier);
                if (claim != null)
                {
                    long userId = Convert.ToInt64(claim.Value);
                    currentUser.SetValue(userId);
                }
            }

            await _next(context);
        }
    }
}
