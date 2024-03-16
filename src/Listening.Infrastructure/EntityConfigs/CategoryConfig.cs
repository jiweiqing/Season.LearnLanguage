using Learning.Infrastructure;
using Listening.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Listening.Infrastructure
{
    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable<long, Category>(null);
            builder.Property(c => c.Name).HasMaxLength(FieldConstants.MaxNameLength).IsRequired();
            builder.Property(c => c.ImageUrl).HasMaxLength(FieldConstants.MaxPathLength).IsRequired();
            builder.Property(c => c.SortOrder).IsRequired();
        }
    }
}
