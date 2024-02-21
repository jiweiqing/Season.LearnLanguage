using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Domain
{
    public interface IJwtService
    {
        /// <summary>
        /// create token
        /// </summary>
        /// <param name="userId">userId</param>
        /// <param name="claims">claims</param>
        /// <returns></returns>
        JwtDto CreateToken(long userId, IEnumerable<Claim> claims);

        /// <summary>
        /// 校验token
        /// </summary>
        /// <param name="accessToken">accessToken</param>
        /// <param name="refreshToken">accessToken</param>
        /// <param name="userId">userId</param>
        /// <returns></returns>
        bool ValidateToken(string accessToken, string refreshToken, out long userId);
    }
}
