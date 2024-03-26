using IdentityService.Domain;
using Learning.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace IdentityService.Host
{
    /// <summary>
    /// 账号相关
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserDomainService _userService;
        private readonly IUserRepository _userRepository;
        public AccountController(UserDomainService userService, IUserRepository userRepository)
        {
            _userService = userService;
            _userRepository = userRepository;
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
        public async Task<ActionResult<JwtDto>> RefreshToken(string refreshToken)
        {
            var accessToken = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            var dto = await _userService.RefreshTokenAsync(accessToken, refreshToken);
            if (dto == null)
            {
                return Unauthorized();
            }
            else
            {
                return Ok(dto);
            }
        }

        /// <summary>
        /// 获取当前登录用户数据
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        [HttpGet("current")]
        [Authorize]
        public async Task<CurrentUser> GetCurrentUserAsync([FromServices] CurrentUserContext userContext)
        {
            var user = await _userRepository.GetAsync(userContext.Id!.Value);
            if (user == null)
            {
                throw new BusinessException("用户未登录!");
            }
            return new CurrentUser
            {
                UserName = user.UserName,
                NickName = user.NickName,
                Email = user.Email
            };
        }
    }
}
