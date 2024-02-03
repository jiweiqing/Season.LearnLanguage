using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Domain
{
    public class User: AggregateRoot
    {
        public User(long id) : base(id)
        {
        }

        public string UserName { get; set; } = string.Empty;
        public string NickName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 头像 
        /// </summary>
        public string? Avatars { get; set; }

        /// <summary>
        /// 上次登录时间
        /// </summary>
        public DateTime? LastLoginTime { set; get; }

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        public void UpdateLastLoginTime()
        {
            LastLoginTime = DateTime.Now;
        }
    }
}
