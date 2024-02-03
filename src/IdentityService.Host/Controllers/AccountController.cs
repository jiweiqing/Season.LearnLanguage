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
        public AccountController() 
        {
        }

        public JwtTokenDto Login(LoginInput input)
        {
            //
            throw new BusinessException();
        }
    }
}
