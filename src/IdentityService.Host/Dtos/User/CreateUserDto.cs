using IdentityService.Domain;
using System.ComponentModel.DataAnnotations;

namespace IdentityService.Host
{
    public class CreateUserDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        [MaxLength(FieldConstants.MaxNameLength)]
        public string UserName { get; set; } = string.Empty; // TODO:不应该有特殊符号
        /// <summary>
        /// 昵称
        /// </summary>
        [Required]
        [MaxLength(FieldConstants.MaxNameLength)]
        public string NickName { get; set; } = string.Empty;
        /// <summary>
        /// 邮箱
        /// </summary>
        [MaxLength(FieldConstants.MaxEmailLength)]
        public string? Email { get; set; } // TODO:合法性校验
        /// <summary>
        /// 密码
        /// </summary>
        // TODO:强度校验
        [Required]
        [MaxLength(FieldConstants.MaxPasswordLength)]
        [StringLength(maximumLength: FieldConstants.MaxPasswordLength, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 头像  暂时不做
        /// </summary>
        //public string? Avatars { get; set; }
    }
}
