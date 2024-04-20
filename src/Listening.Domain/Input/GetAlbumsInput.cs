using Learning.Domain;

namespace Listening.Domain
{
    public class GetAlbumsInput: GetAlbumsBaseInput
    {
        /// <summary>
        /// 是否可见
        /// </summary>
        public bool? IsEabled { get; set; }
    }

    public class GetAlbumsBaseInput: PagedInput
    {
        /// <summary>
        /// 分类id
        /// </summary>
        public long? CategoryId { get; set; }
        /// <summary>
        /// 依据名称查询
        /// </summary>
        public string? Name { get; set; }
    }
}
