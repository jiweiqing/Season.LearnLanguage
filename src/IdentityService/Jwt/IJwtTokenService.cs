using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Infrastructure
{
    public interface IJwtTokenService
    {
        /// <summary>
        /// create token
        /// </summary>
        /// <param name="userId">userId</param>
        /// <param name="claims">claims</param>
        /// <returns></returns>
        JwtTokenDto CreateToken(long userId, IEnumerable<Claim> claims);

        /// <summary>
        /// get user id by refresh token
        /// </summary>
        /// <param name="refreshToken">refreshToken</param>
        /// <returns></returns>
        long GetUserIdByRefreshToken(string refreshToken);
    }
}
