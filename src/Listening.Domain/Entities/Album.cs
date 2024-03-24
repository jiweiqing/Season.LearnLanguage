using Learning.Domain;
using Yitter.IdGenerator;

namespace Listening.Domain
{
    /// <summary>
    /// 专辑
    /// </summary>
    public class Album : AggregateRoot
    {
        public Album(long id) : base(id)
        {
        }

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

        /// <summary>
        /// 描述
        /// </summary>
        public string? Description {  get; set; }

        public static Album Create(int sortOrder, string name, long categoryId, string? description)
        {
            Album album = new Album(YitIdHelper.NextId())
            {
                SortOrder = sortOrder,
                Name = name,
                IsEabled = false,
                CategoryId = categoryId,
                Description = description
            };
            return album;
        }

        public Album Update(int sortOrder, string name, long categoryId,string? description)
        {
            this.CategoryId = categoryId;
            this.Name = name;
            this.SortOrder = sortOrder;
            this.Description = description;
            return this;
        }

        /// <summary>
        /// 改变状态,是否可用
        /// </summary>
        /// <param name="isEabled"></param>
        /// <returns></returns>
        public Album ChangeStatus(bool isEabled)
        {
            this.IsEabled = isEabled;
            return this;
        }
    }
}
