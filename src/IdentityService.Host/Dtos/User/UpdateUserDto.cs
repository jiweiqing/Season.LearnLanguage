using IdentityService.Domain;
using System.ComponentModel.DataAnnotations;

namespace IdentityService.Host
{
    public class UpdateUserDto
    {
        /// <summary>
        /// 昵称
        /// </summary>
        [MaxLength(FieldConstants.MaxNameLength)]
        [Required]
        public string NickName { get; set; } = string.Empty;
        /// <summary>
        /// 邮箱
        /// </summary>
        [MaxLength(FieldConstants.MaxEmailLength)]
        public string? Email { get; set; }
    }
}
