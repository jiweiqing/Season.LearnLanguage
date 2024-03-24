using Learning.AspNetCore;

namespace Listening.Admin.Host
{
    public class AlbumDto: CreationEntityDto
    {
        /// <summary>
        /// 是否可用
        /// </summary>
        public bool IsEabled { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 排序
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// 分类id
        /// </summary>
        public long CategoryId { get; set; }
    }
}
