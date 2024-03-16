using FileService.Domain;
using Microsoft.EntityFrameworkCore;

namespace FileService.Infrastructure
{
    public class FileRepository : IFileRepository
    {
        private readonly FileServiceDbContext _dbContext;
        public FileRepository(FileServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task<FileUploadRecord?> FindFileAsync(long fileSize, string hash)
        {
            return _dbContext.FileUploadRecords.FirstOrDefaultAsync(u => u.FileByteSize == fileSize
            && u.FileHash == hash);
        }
    }
}
