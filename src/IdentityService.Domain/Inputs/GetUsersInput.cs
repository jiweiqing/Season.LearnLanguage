using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Domain
{
    public class GetUsersInput: PagedInput
    {
        public string? UserName { get; set; }
        public string? NickName { get; set; }
        public string? Email { get; set; }
    }
}
