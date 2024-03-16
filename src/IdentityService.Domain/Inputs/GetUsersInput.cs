using Learning.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Domain
{
    /// <summary>
    /// 查询用户输入
    /// </summary>
    public class GetUsersInput: PagedInput
    {
        public string? UserName { get; set; }
        public string? NickName { get; set; }
        public string? Email { get; set; }
    }
}
