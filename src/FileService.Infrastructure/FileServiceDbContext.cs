using FileService.Domain;
using Learning.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace FileService.Infrastructure
{
    public class FileServiceDbContext: DbContextBase
    {
        public FileServiceDbContext(DbContextOptions<FileServiceDbContext> options) : base(options)
        {
        }

        #region DbSets

        public DbSet<FileUploadRecord> FileUploadRecords { get; set; }

        #endregion
    }
}
