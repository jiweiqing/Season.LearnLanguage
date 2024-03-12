using IdentityService.Domain;
using Learning.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Host
{
    /// <summary>
    /// 账号相关
    /// </summary>
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserDomainService _userService;
        public AccountController(UserDomainService userService) 
        {
            _userService = userService;
        }

        /// <summary>
        /// 用户名密码登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<ActionResult<JwtDto>> Login(LoginInput input)
        {
            var token = await _userService.LoginByUserNameAsync(input.UserName, input.Password);
            return Ok(token);
        }

        /// <summary>
        /// 刷新token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        [HttpGet("refresh-token")]
        public async Task<JwtDto> RefreshToken(string refreshToken)
        {
            var accessToken = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            return await _userService.RefreshTokenAsync(accessToken, refreshToken);
        }
    }
}
