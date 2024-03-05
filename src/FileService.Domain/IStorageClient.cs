using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService.Domain
{
    public interface IStorageClient
    {
        StorageType StorageType { get; }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="key">文件的保存路径</param>
        /// <param name="stream">文件内容</param>
        /// <param name="cancellationToken"></param>
        /// <returns>保存的文件</returns>
        Task<string> SaveAsync(string key,Stream stream,CancellationToken cancellationToken = default);
    }
}
