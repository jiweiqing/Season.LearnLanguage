using Listening.Domain;
using System.ComponentModel.DataAnnotations;

namespace Listening.Admin.Host.Dtos.Episodes
{
    /// <summary>
    /// 只允许修改名称和字幕
    /// </summary>
    public class UpdateEpisodeDto
    {
        /// <summary>
        /// 标题
        /// </summary>
        [Required]
        [MaxLength(FieldConstants.MaxNameLength)]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// 字幕
        /// </summary>
        [Required]
        public string Subtitle { get; set; } = string.Empty;
        /// <summary>
        /// 字幕类型
        /// </summary>
        [Required]
        public SubtitleType SubtitleType { get; set; }
    }
}
