using Learning.Domain;

namespace Listening.Domain
{
    public interface IAlbumRepository : IRepository<long, Album>, IScopedDependency
    {
        /// <summary>
        /// 获取专辑列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<Album>> GetListAsync(GetAlbumsInput input);

        /// <summary>
        /// 获取总条数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<int> CountAsync(GetAlbumsInput input);

        /// <summary>
        /// 获取最大的排序
        /// </summary>
        /// <returns></returns>
        Task<int> GetMaxSortOrderAsync();
    }
}
