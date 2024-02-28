using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Yitter.IdGenerator;

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

        /// <summary>
        /// 版本号(用于撤回jwt)
        /// </summary>
        public int JwtVersion { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();


        public static User Create(string userName, string nickName, string? email, string password)
        {
            User user = new User(YitIdHelper.NextId())
            {
                UserName = userName,
                NickName = nickName,
                Email = email,
                Password = EncryptHelper.MD5Encrypt(password)
            };

            return user;
        }

        public User Update(string nickName,string? email)
        {
            this.NickName = nickName;
            this.Email = email;
            return this;
        }

        public void UpdateLastLoginTime()
        {
            LastLoginTime = DateTime.Now;
        }
        public void UpdateVersion()
        {
            JwtVersion++;
        }
    }
}
