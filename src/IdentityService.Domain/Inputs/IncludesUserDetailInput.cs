using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Domain
{
    public class IncludesUserDetailInput
    {
        /// <summary>
        /// 包含用户角色
        /// </summary>
        public bool IncludeUserRole { get; set; }
    }
}
