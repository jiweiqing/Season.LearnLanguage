using Learning.Domain;

namespace Listening.Domain
{
    public class GetCategoriesInput : PagedInput
    {
        /// <summary>
        /// 类别名称
        /// </summary>
        public string? Name { get; set; }
    }
}
