using IdentityService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Learning.Domain;

namespace IdentityService.Infrastructure
{
    public class IdentityServiceDbContext:DbContext
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
        public IdentityServiceDbContext(DbContextOptions<IdentityServiceDbContext> options):base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
            //modelBuilder.EnableSoftDeletionGlobalFilter();
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// 重写SaveChangesAsync,给审计属性赋值
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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

        #region DeSet

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        #endregion

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
            if (entry is IFullAuditedObject fullAuditedObject && !fullAuditedObject.IsDeleted)
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
