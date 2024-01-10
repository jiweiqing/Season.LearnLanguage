using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Domain
{
    public class UserRole
    {
        public long UserId {  get; set; }
        public long RoleId { get; set; }
    }
}
