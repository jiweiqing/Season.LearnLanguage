using MediaEncoder.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Learning.Infrastructure;

namespace MediaEncoder.Infrastructure
{
    public class EncodingItemConfig : IEntityTypeConfiguration<EncodingItem>
    {
        public void Configure(EntityTypeBuilder<EncodingItem> builder)
        {
            builder.ToTable<long, EncodingItem>(null);
            builder.Property(e => e.FileName).HasMaxLength(FieldConstants.MaxNameLength).IsRequired();
            builder.Property(e => e.FileHash).HasMaxLength(64);
            builder.Property(e => e.OutputFormat).HasMaxLength(FieldConstants.MaxCodeLength).IsRequired();
            builder.Property(e => e.Status).HasMaxLength(FieldConstants.MaxCodeLength).HasConversion<string>().IsRequired();
            builder.Property(e => e.SourceSystem).HasMaxLength(FieldConstants.MaxNameLength).IsRequired();
            builder.Property(e => e.LogText).HasMaxLength(FieldConstants.MaxDescriptionLength);
            builder.Property(e => e.OutputUrl).HasMaxLength(FieldConstants.MaxDescriptionLength);
            builder.Property(e => e.SourceUrl).HasMaxLength(FieldConstants.MaxDescriptionLength);
        }
    }
}
