using Listening.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Learning.Infrastructure;

namespace Listening.Infrastructure
{
    public class AlbumConfig : IEntityTypeConfiguration<Album>
    {
        public void Configure(EntityTypeBuilder<Album> builder)
        {
            builder.ToTable<long, Album>(null);
            builder.Property(a => a.Name).HasMaxLength(FieldConstants.MaxNameLength).IsRequired();
            builder.Property(a => a.IsEabled).IsRequired();
            builder.Property(c => c.SortOrder).IsRequired();
            builder.Property(c => c.CategoryId).IsRequired();
            builder.Property(c => c.Description).HasMaxLength(FieldConstants.MaxDescriptionLength);

            builder.HasIndex(a => a.Name);
            builder.HasIndex(a => a.CategoryId);
        }
    }
}
