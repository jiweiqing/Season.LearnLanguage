using Learning.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Learning.Infrastructure
{
    public class DbContextBase : DbContext
    {
        private CurrentUserContext? _currentUserContext;
        public CurrentUserContext CurrentUserContext
        {
            get
            {
                if (_currentUserContext == null && this is IInfrastructure<IServiceProvider> serviceProvider)
                {
                    _currentUserContext = serviceProvider.GetService<CurrentUserContext>();
                }
                return _currentUserContext;
            }
        }

        protected DbContextBase(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
            //modelBuilder.EnableSoftDeletionGlobalFilter();
            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            foreach (EntityEntry entityEntry in ChangeTracker.Entries())
            {
                var entity = entityEntry.Entity;
                switch (entityEntry.State)
                {
                    case EntityState.Added:
                        SetCreaiondAudit(entity);
                        break;
                    case EntityState.Modified:
                        SetModificationAudit(entity);
                        break;
                    case EntityState.Deleted:
                        SetDeletionAudit(entityEntry);
                        break;
                    default:
                        break;
                }
            }
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        #region private methods

        private void SetCreaiondAudit(object entity)
        {
            if (entity is ICreationAuditedObject auditedObject)
            {
                auditedObject.CreationTime = DateTime.Now;
                if (CurrentUserContext != null && CurrentUserContext.Id.HasValue)
                {
                    auditedObject.CreatorId = CurrentUserContext.Id;
                }
            }
        }

        private void SetModificationAudit(object entity)
        {
            if (entity is IAuditedObject auditedObject)
            {
                auditedObject.ModificationTime = DateTime.Now;
                if (CurrentUserContext != null && CurrentUserContext.Id.HasValue)
                {
                    auditedObject.ModifierId = CurrentUserContext.Id;
                }
            }
        }

        private void SetDeletionAudit(EntityEntry entry)
        {
            // TODO:可能有问题 应该是entry.Entity?
            if (entry.Entity is IFullAuditedObject fullAuditedObject && !fullAuditedObject.IsDeleted)
            {
                entry.State = EntityState.Modified;
                fullAuditedObject.IsDeleted = true;
                fullAuditedObject.DeletionTime = DateTime.Now;
                if (CurrentUserContext != null && CurrentUserContext.Id != null)
                {
                    fullAuditedObject.DeleterId = CurrentUserContext.Id;
                }
            }
        }

        #endregion
    }
}
