using Learning.AspNetCore;

namespace Listening.Admin.Host
{
    public class CategoryDto: CreationEntityDto
    {
        /// <summary>
        /// 排序
        /// </summary>
        public int SortOrder { get; set; }
        /// <summary>
        /// 类别名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 图片路径
        /// </summary>
        public string ImageUrl { get; set; } = string.Empty;
    }
}
