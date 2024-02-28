using IdentityService.Domain;
using IdentityService.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityService.Infrastructure
{
    public class JwtService : IJwtService
    {
        private readonly JwtOptions _jwtTokenOptions;
        public JwtService(IOptions<JwtOptions> jwtTokenOptions)
        {
            _jwtTokenOptions = jwtTokenOptions.Value;
        }
        public JwtDto CreateToken(long userId, IEnumerable<Claim> claims)
        {
            string accessToken = CreateAccessToken(claims);
            string refreshToken = CreateRefreshToken(claims.Where(c => c.Type == JwtRegisteredClaimNames.Sub || c.Type == IdentityContants.Version));

            return new JwtDto()
            {
                AccessToken = accessToken,
                AccessTokenExpiration = _jwtTokenOptions.AccessTokenExpiration,
                RefreshToken = refreshToken,
                RefreshTokenExpiration = _jwtTokenOptions.RefreshTokenExpiration
            };
        }

        /// <summary>
        /// 校验token
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="refreshToken"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool ValidateToken(string accessToken, string refreshToken, out long userId)
        {
            userId = 0;
            // 1.校验access token合法性（不校验过期时间）
            TokenValidationParameters validationParameters = new()
            {
                ValidateAudience = true,
                //ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtTokenOptions.Issuer,
                ValidAudience = _jwtTokenOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenOptions.SecurityKey)),
                ClockSkew = TimeSpan.FromMinutes(1)
            };

            JwtSecurityTokenHandler handler = new();

            try
            {
                _ = handler.ValidateToken(accessToken, validationParameters, out _);
            }
            catch (Exception)
            {
                return false;
            }

            // 2.校验refresh token合法性（校验过期时间）
            TokenValidationParameters validationRefreshParameters = new()
            {
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtTokenOptions.Issuer,
                ValidAudience = _jwtTokenOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenOptions.RefreshSecurityKey)),
                ClockSkew = TimeSpan.FromMinutes(1)
            };

            JwtSecurityTokenHandler refreshTokenHandler = new();

            ClaimsPrincipal? principal;
            try
            {
                principal = refreshTokenHandler.ValidateToken(refreshToken, validationRefreshParameters, out _);
            }
            catch (Exception)
            {
                return false;
            }

            Claim? claim = principal.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null && long.TryParse(claim.Value, out userId))
            {
                return true;
            }

            return false;
        }

        private string CreateAccessToken(IEnumerable<Claim> claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenOptions.SecurityKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var utcNow = DateTime.UtcNow;

            var securityToken = new JwtSecurityToken(
                _jwtTokenOptions.Issuer,
                _jwtTokenOptions.Audience,
                claims,
                notBefore: utcNow,
                expires: utcNow.AddSeconds(_jwtTokenOptions.AccessTokenExpiration),
                signingCredentials: credentials);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return accessToken;
        }

        private string CreateRefreshToken(IEnumerable<Claim> claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenOptions.RefreshSecurityKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var utcNow = DateTime.UtcNow;

            var securityToken = new JwtSecurityToken(
                _jwtTokenOptions.Issuer,
                _jwtTokenOptions.Audience,
                claims,
                notBefore: utcNow,
                expires: utcNow.AddSeconds(_jwtTokenOptions.RefreshTokenExpiration),
                signingCredentials: credentials);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return accessToken;
        }
    }
}
