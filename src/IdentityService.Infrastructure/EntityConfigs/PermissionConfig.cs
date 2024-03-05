using IdentityService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Learning.Infrastructure;

namespace IdentityService.Infrastructure
{
    class PermissionConfig : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable<long, Permission>(null);

            builder.Property(p => p.Name).HasMaxLength(FieldConstants.MaxNameLength).IsRequired();
            builder.Property(p => p.Code).HasMaxLength(FieldConstants.MaxPasswordLength).IsRequired();

            // indexs 
            builder.HasIndex(p => p.Name);
            builder.HasIndex(p => p.Code);
        }
    }
}
