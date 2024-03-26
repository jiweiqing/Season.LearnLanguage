using Listening.Domain;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Listening.Admin.Host
{
    public class CreateEpisodeDto
    {
        /// <summary>
        /// 专辑id
        /// </summary>
        [Required]
        public long AlbumId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        [Required]
        [MaxLength(FieldConstants.MaxNameLength)]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// 音频资源url
        /// </summary>
        [Required]
        [MaxLength(FieldConstants.MaxPathLength)]
        public string Resource { get; set; } = string.Empty;
        /// <summary>
        /// 时长,单位秒
        /// </summary>
        [Required]
        public double Duration { get; set; }
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
