using Learning.Domain;
namespace Listening.Domain
{
    public interface ICategoryRepository: IRepository<long, Category>, IScopedDependency
    {
        /// <summary>
        /// 获取总条数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<int> CountAsync(GetCategoriesInput input);
        /// <summary>
        /// 获取分类列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<Category>> GetListAsync(GetCategoriesInput input);
    }
}
