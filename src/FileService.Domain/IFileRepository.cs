using Learning.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService.Domain
{
    public interface IFileRepository : IScopedDependency
    {
        /// <summary>
        /// 依据文件大小以及哈希值查找文件
        /// </summary>
        /// <param name="fileSize"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        Task<FileUploadRecord?> FindFileAsync(long fileSize, string hash);
    }
}
