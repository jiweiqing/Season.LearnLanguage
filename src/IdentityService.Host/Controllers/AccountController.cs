using IdentityService.Domain;
using IdentityService.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Host
{
    [Route("api/[controller]")]
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
        public async Task<ActionResult<JwtTokenDto>> Login(LoginInput input)
        {
            var token = await _userService.LoginByUserName(input.UserName, input.Password);
            return Ok(token);
        }
    }
}
