using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Domain
{
    public class UserDomainService
    {
        private readonly IUserRepository _repository;
        private readonly IJwtTokenService _jwtService;
        public UserDomainService(IUserRepository repository, IJwtTokenService jwtService)
        {
            _repository = repository;
            _jwtService = jwtService;
        }

        public async Task<JwtTokenDto> LoginByUserName(string userName, string password)
        {
            // TODO: 发布领域事件登录日志成功后者失败
            User? user = await _repository.GetUserByName(userName);
            if (user == null)
            {
                throw new BusinessException("用户名或密码错误!");
            }
            if (user.Password != EncryptHelper.MD5Encrypt(password))
            {
                throw new BusinessException("用户名或密码错误!");
            }

            user.UpdateLastLoginTime();

            await _repository.UpdateAsync(user);

            long iat = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString(),ClaimValueTypes.String),
                new Claim(JwtRegisteredClaimNames.Sub,user.Id.ToString(),ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Iat, iat.ToString(), ClaimValueTypes.Integer64)
            };

            return _jwtService.CreateToken(user.Id, claims);
        }
    }
}
