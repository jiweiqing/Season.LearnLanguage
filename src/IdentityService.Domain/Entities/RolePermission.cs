using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Domain
{
    public class RolePermission
    {
        public long RoleId {  get; set; }
        public long PermissionId {  get; set; }
    }
}
