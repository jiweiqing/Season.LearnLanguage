using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Domain
{
    /// <summary>
    /// 包含导航属性
    /// </summary>
    public class IncludesUsersInput: GetUsersInput
    {
        /// <summary>
        /// 包含用户角色
        /// </summary>
        public bool IncludeUserRole { get; set; }
    }
}
