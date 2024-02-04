using IdentityService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace IdentityService.Infrastructure
{
    public static class EntityTypeBuilderExtensions
    {
        public static EntityTypeBuilder ToTable<TKey, TEntity>(this EntityTypeBuilder builder, string? prefix = null) where TEntity : EntityBase<TKey>
        {
            ToTable<TEntity>(builder, prefix);
            //Type entityType = builder.Metadata.ClrType;
            var entityType = typeof(TEntity);
            // 主键
            builder.HasKey(nameof(EntityBase<TKey>.Id));

            if (entityType.IsAssignableTo(typeof(IFullAuditedObject)))
            {
                builder.Property(nameof(IFullAuditedObject.IsDeleted));
                Expression<Func<TEntity, bool>> expression = e => !EF.Property<bool>(e, nameof(IFullAuditedObject.IsDeleted));
                builder.HasQueryFilter(expression);
            }

            return builder;
        }

        public static EntityTypeBuilder ToTable<TEntity>(
            this EntityTypeBuilder builder,
            string? prefix = null)
            where TEntity : class
        {
            var type = typeof(TEntity);
            // 需要优化 "" 
            var tableName = prefix == null ? type.Name : $"{prefix}{type.Name}";
            builder.ToTable(tableName);
            return builder;
        }
    }
}
