using Learning.Domain;

namespace Listening.Domain
{
    public interface IEpisodeRepository: IRepository<long, Episode>, IScopedDependency
    {
        /// <summary>
        /// 获取总条数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<int> CountAsync(GetEpisodesInput input);
        /// <summary>
        /// 获取音频列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<Episode>> GetListAsync(GetEpisodesInput input);
        /// <summary>
        /// 获取最大排序
        /// </summary>
        /// <returns></returns>
        Task<int> GetMaxSortOrderAsync();
    }
}
