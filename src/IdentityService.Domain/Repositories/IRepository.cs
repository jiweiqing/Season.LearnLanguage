using System.Linq.Expressions;

namespace IdentityService.Domain
{
    /// <summary>
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否自动保存</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>TEntity</returns>
        Task<TEntity> InsertAsync(
            TEntity entity,
            bool autoSave = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否自动保存</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>TEntity</returns>
        Task<TEntity> UpdateAsync(
            TEntity entity,
            bool autoSave = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否自动保存</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>TEntity</returns>
        Task DeleteAsync(
            TEntity entity,
            bool autoSave = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="predicate">表达式</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>TEntity</returns>
        Task<TEntity?> GetAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="predicate">表达式</param>
        /// <param name="cancellationToken"></param>
        /// <returns>List<TEntity>/returns>
        Task<List<TEntity>> GetListAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取总数
        /// </summary>
        /// <param name="predicate">表达式</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> CountAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TKey, TEntity> : IRepository<TEntity> where TEntity : class
    {
        Task DeleteAsync(
            TKey id,
            bool autoSave,
            CancellationToken cancellationToken = default);
        Task<TEntity?> GetAsync(
            TKey id,
            CancellationToken cancellationToken = default);
    }
}
