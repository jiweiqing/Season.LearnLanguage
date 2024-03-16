using IdentityService.Domain;
using Learning.AspNetCore;

namespace IdentityService.Host
{
    public class UserDto: EntityDtoBase
    {
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
    }
}
