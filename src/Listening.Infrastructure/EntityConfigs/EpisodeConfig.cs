using Learning.Infrastructure;
using Listening.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Listening.Infrastructure
{
    public class EpisodeConfig : IEntityTypeConfiguration<Episode>
    {
        public void Configure(EntityTypeBuilder<Episode> builder)
        {
            builder.ToTable<long, Episode>(null);
            builder.Property(e => e.Name).HasMaxLength(FieldConstants.MaxNameLength).IsRequired();
            builder.Property(e => e.SubtitleType).HasMaxLength(FieldConstants.MaxCodeLength)
                .HasConversion<string>().IsRequired();
            builder.Property(e => e.Resource).HasMaxLength(FieldConstants.MaxPathLength).IsRequired();
            builder.Property(e => e.Subtitle).IsRequired();

            builder.HasIndex(e => e.Name);
            builder.HasIndex(e => e.AlbumId);
        }
    }
}
