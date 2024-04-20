using Learning.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Yitter.IdGenerator;

namespace Listening.Domain
{
    /// <summary>
    /// 类别
    /// </summary>
    public class Category : AggregateRoot
    {
        public Category(long id) : base(id)
        {
        }

        /// <summary>
        /// 排序
        /// </summary>
        public int SortOrder {  get; set; }
        /// <summary>
        /// 类别名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 图片路径
        /// </summary>
        public string ImageUrl { get; set; } = string.Empty;

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="sortOrder">排序</param>
        /// <param name="name">名称</param>
        /// <param name="imageUrl">图片路径</param>
        /// <returns></returns>
        public static Category Create(int sortOrder,string name,string imageUrl)
        {
            Category category = new Category(YitIdHelper.NextId())
            {
                SortOrder = sortOrder,
                Name = name,
                ImageUrl = imageUrl
            };

            return category;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="imageUrl">图片路径</param>
        /// <returns></returns>
        public Category Update(string name, string imageUrl)
        {
            this.Name = name;
            this.ImageUrl = imageUrl;
            return this;
        }
    }
}
