using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Domain
{
    public class RolePermission
    {
        public RolePermission(long roleId,long permissionId)
        {
            RoleId = roleId;
            PermissionId = permissionId;
        }
        public long RoleId {  get; set; }
        public long PermissionId {  get; set; }
    }
}
