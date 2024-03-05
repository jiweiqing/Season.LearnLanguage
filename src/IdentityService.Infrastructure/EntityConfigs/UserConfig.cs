using IdentityService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Learning.Infrastructure;

namespace IdentityService.Infrastructure
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable<long,User>(null);
            builder.Property(u => u.UserName).HasMaxLength(FieldConstants.MaxNameLength).IsRequired();
            builder.Property(u => u.NickName).HasMaxLength(FieldConstants.MaxNameLength).IsRequired();
            builder.Property(u => u.Password).HasMaxLength(FieldConstants.MaxPasswordLength).IsRequired();
            builder.Property(u => u.Avatars).HasMaxLength(FieldConstants.MaxPathLength);
            builder.Property(u => u.Email).HasMaxLength(FieldConstants.MaxPathLength);

            // indexs 
            builder.HasIndex(u => u.UserName);
            builder.HasIndex(u => u.NickName);
            builder.HasIndex(u => u.Email);
        }
    }
}
