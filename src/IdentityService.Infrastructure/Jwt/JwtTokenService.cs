using IdentityService.Domain;
using IdentityService.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityService.Infrastructure
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtTokenOptions _jwtTokenOptions;
        public JwtTokenService(IOptions<JwtTokenOptions> jwtTokenOptions)
        {
            _jwtTokenOptions = jwtTokenOptions.Value;
        }
        public JwtTokenDto CreateToken(long userId, IEnumerable<Claim> claims)
        {
            string accessToken = CreateAccessToken(claims);
            //string refreshToken = CreateRefreshToken(userId);

            return new JwtTokenDto()
            {
                AccessToken = accessToken,
                AccessTokenExpiration = _jwtTokenOptions.AccessTokenExpiration,
                //RefreshToken = refreshToken,
                RefreshTokenExpiration = _jwtTokenOptions.RefreshTokenExpiration
            };
        }

        public long GetUserIdByRefreshToken(string refreshToken)
        {
            throw new NotImplementedException();
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

        // TODO: refresh token
    }
}
