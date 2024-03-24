using Learning.Domain;

namespace Listening.Domain
{
    public class GetEpisodesInput: PagedInput
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 字幕类型
        /// </summary>
        public SubtitleType? SubtitleType { get; set; }
    }
}
