using IdentityService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityService.Infrastructure
{
    class RoleConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable<long,Role>(null);
            builder.Property(ur => ur.Name).HasMaxLength(FieldConstants.MaxNameLength).IsRequired();
            builder.Property(ur => ur.Code).HasMaxLength(FieldConstants.MaxPasswordLength).IsRequired();

            // indexs 
            builder.HasIndex(ur => ur.Name);
        }
    }
}
