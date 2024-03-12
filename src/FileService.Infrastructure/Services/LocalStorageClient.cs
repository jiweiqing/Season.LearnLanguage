using FileService.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService.Infrastructure
{
    public class LocalStorageClient : IStorageClient
    {
        public StorageType StorageType => StorageType.Backup;

        private readonly IWebHostEnvironment _hostEnvironment;
        public LocalStorageClient(IOptions<LocalStorageOptions> options , IWebHostEnvironment webHostEnvironment) 
        {
            _hostEnvironment = webHostEnvironment;
        }
        /// <summary>
        /// 保存本地目录
        /// </summary>
        /// <param name="key"></param>
        /// <param name="stream"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<string> SaveAsync(string key, Stream stream, CancellationToken cancellationToken = default)
        {
            if (key.StartsWith('/'))
            {
                throw new ArgumentException("key should not start with /", nameof(key));
            }
            string workingDir = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot");
            string fullPath = Path.Combine(workingDir, key);
            string? fullDir = Path.GetDirectoryName(fullPath);//get the directory
            if (!Directory.Exists(fullDir))//automatically create dir
            {
                Directory.CreateDirectory(fullDir!);
            }
            if (File.Exists(fullPath))//如果已经存在，则尝试删除
            {
                File.Delete(fullPath);
            }
            using Stream outStream = File.OpenWrite(fullPath);
            await stream.CopyToAsync(outStream, cancellationToken);
            return key;
        }
    }
}
