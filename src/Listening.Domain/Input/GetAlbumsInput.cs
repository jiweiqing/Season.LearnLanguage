using Learning.Domain;

namespace Listening.Domain
{
    public class GetAlbumsInput: PagedInput
    {
        /// <summary>
        /// 依据名称查询
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 是否可见
        /// </summary>
        public bool? IsEabled { get; set; }
    }
}
