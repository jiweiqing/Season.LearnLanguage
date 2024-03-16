using Learning.Domain;
using Learning.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Learning.Infrastructure
{
    public class Repository<TEntity> : IRepository<TEntity>, IScopedDependency where TEntity : class
    {
        public Repository(DbContextBase context)
        {
            EfContext = context;
        }

        // TODO:改为dbcontext的基类
        public DbContextBase EfContext { get; }

        public DbSet<TEntity> DbSet => EfContext.Set<TEntity>();

        public async Task<TEntity> InsertAsync(
            TEntity entity,
            bool autoSave = false,
            CancellationToken cancellationToken = default)
        {
            await DbSet.AddAsync(entity, cancellationToken);
            if (autoSave)
            {
                await EfContext.SaveChangesAsync(cancellationToken);
            }
            return entity;
        }

        public async Task<TEntity> UpdateAsync(
            TEntity entity,
            bool autoSave = false,
            CancellationToken cancellationToken = default)
        {
            DbSet.Update(entity);
            if (autoSave)
            {
                await EfContext.SaveChangesAsync(cancellationToken);
            }
            return entity;
        }

        public async Task DeleteAsync(
            TEntity entity,
            bool autoSave = false,
            CancellationToken cancellationToken = default)
        {
            DbSet.Remove(entity);
            if (autoSave)
            {
                await EfContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<TEntity?> GetAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await DbSet.Where(predicate).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<TEntity>> GetListAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await DbSet.Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await DbSet.Where(predicate).CountAsync(cancellationToken);
        }
    }


    public class Repository<TKey, TEntity>
        : Repository<TEntity>, IRepository<TKey, TEntity> where TEntity : EntityBase<TKey>
    {
        public Repository(DbContextBase context) : base(context)
        {
        }

        public async Task DeleteAsync(
            TKey id,
            bool autoSave = false,
            CancellationToken cancellationToken = default)
        {
            TEntity? entity = await DbSet.FindAsync(new object[] { id! }, cancellationToken);
            if (entity != null)
            {
                await DeleteAsync(entity, false, cancellationToken);
            }
        }

        public async Task<TEntity?> GetAsync(
            TKey id,
            CancellationToken cancellationToken = default)
        {
            return await DbSet.FindAsync(new object[] { id! }, cancellationToken);
        }
    }
}
