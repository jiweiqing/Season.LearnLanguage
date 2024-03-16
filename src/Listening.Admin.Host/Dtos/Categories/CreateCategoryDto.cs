using Listening.Domain;
using System.ComponentModel.DataAnnotations;

namespace Listening.Admin.Host
{
    public class CreateCategoryDto
    {
        /// <summary>
        /// 显示顺序
        /// </summary>
        [Required]
        public int SortOrder {  get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        [Required]
        [MaxLength(FieldConstants.MaxNameLength)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 封面url
        /// </summary>
        [Required]
        [MaxLength(FieldConstants.MaxNameLength)]
        public string ImageUrl { get; set; } = string.Empty;
    }
}
