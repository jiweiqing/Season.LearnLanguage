using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Listening.Admin.Host.Controllers
{
    [Route("listening/api/[controller]")]
    [ApiController]
    public class ListeningControllerBase : ControllerBase
    {
    }
}
