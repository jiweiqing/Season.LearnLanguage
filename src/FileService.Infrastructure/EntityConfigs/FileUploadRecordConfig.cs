using FileService.Domain;
using Learning.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileService.Infrastructure
{
    public class FileUploadRecordConfig : IEntityTypeConfiguration<FileUploadRecord>
    {
        public void Configure(EntityTypeBuilder<FileUploadRecord> builder)
        {
            builder.ToTable<long, FileUploadRecord>(null);
            builder.Property(f => f.FileByteSize).IsRequired();
            builder.Property(f => f.FileName).HasMaxLength(FileFieldConstants.MaxFileNameLength).IsRequired();
            builder.Property(f => f.FileHash).HasMaxLength(FileFieldConstants.MaxFileNameLength).IsRequired();
            builder.Property(f => f.BackupUrl).HasMaxLength(FileFieldConstants.MaxFileUrlLength);
            builder.Property(f => f.RemoteUrl).HasMaxLength(FileFieldConstants.MaxFileUrlLength).IsRequired();

            // indexs 
            builder.HasIndex(u => u.FileByteSize);
            builder.HasIndex(u => u.FileHash);
        }
    }
}
