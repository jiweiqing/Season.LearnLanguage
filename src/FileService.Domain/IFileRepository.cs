using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService.Domain
{
    public interface IFileRepository
    {
        Task<FileUploadRecord> FindFileAsync(long fileSize,string hash);
    }
}
