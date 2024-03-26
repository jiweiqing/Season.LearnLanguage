using Learning.Domain;

namespace Listening.Domain
{
    public class GetEpisodesInput: PagedInput
    {
        /// <summary>
        /// 专辑id
        /// </summary>
        public long? AlbumId {  get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 字幕类型
        /// </summary>
        public SubtitleType? SubtitleType { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool? IsEnable { get; set; }
    }
}
