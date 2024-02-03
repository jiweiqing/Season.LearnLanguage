using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Domain
{
    public class Role : AggregateRoot
    {
        public Role(long id) : base(id)
        {
        }

        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public ICollection<RolePermission> RolePermissions { get; set;} = new List<RolePermission>();
    }
}
