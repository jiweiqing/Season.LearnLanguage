using FileService.Domain;
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
        private IOptions<LocalStorageOptions> _options {  get; set; }
        public LocalStorageClient(IOptions<LocalStorageOptions> options) 
        {
            _options = options;
        }
        public StorageType StorageType => StorageType.Backup;

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

            string workingDir = _options.Value.WorkingDir;
            string fullPath = Path.Combine(workingDir, key);
            string? fullDir = Path.GetDirectoryName(fullPath);//get the directory
            if (!Directory.Exists(fullDir))
            {
                Directory.CreateDirectory(fullDir!);
            }

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
            using Stream outStream = File.OpenWrite(fullPath);
            await stream.CopyToAsync(outStream);
            return fullPath;
        }
    }
}
