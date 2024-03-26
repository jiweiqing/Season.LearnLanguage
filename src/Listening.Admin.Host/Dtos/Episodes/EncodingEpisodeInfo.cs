using Listening.Domain;

namespace Listening.Admin.Host
{
    public class EncodingEpisodeInfo
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id {  get; set; }
        /// <summary>
        /// 专辑id
        /// </summary>
        public long AlbumId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// 音频资源url
        /// </summary>
        public string Resource { get; set; } = string.Empty;
        /// <summary>
        /// 时长,单位秒
        /// </summary>
        public double Duration { get; set; }
        /// <summary>
        /// 字幕
        /// </summary>
        public string Subtitle { get; set; } = string.Empty;
        /// <summary>
        /// 字幕类型
        /// </summary>
        public SubtitleType SubtitleType { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string Status {  get; set; } = string.Empty;

    }
}
