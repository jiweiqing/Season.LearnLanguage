using Learning.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FileService.Domain
{
    public class FileUploadRecord : CreationEntity
    {
        public FileUploadRecord(long id) : base(id)
        {
        }

        /// <summary>
        /// 文件大小 字节
        /// </summary>
        public long FileByteSize { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// 哈希值（使用SHA256）
        /// </summary>
        public string FileHash { get; set; } = string.Empty;

        /// <summary>
        /// 备份服务器url
        /// </summary>
        public string? BackupUrl { get; set; }

        /// <summary>
        /// 远程服务器url
        /// </summary>
        public string RemoteUrl { get; set; } = string.Empty;

        public static FileUploadRecord Create(
            long id, 
            long fileByteSize, 
            string fileName, 
            string fileHash, 
            string? backupUrl, 
            string remoteUrl)
        {
            FileUploadRecord item = new FileUploadRecord(id)
            {
                //CreationTime = DateTime.Now,
                FileName = fileName,
                FileHash = fileHash,
                FileByteSize = fileByteSize,
                BackupUrl = backupUrl,
                RemoteUrl = remoteUrl
            };
            return item;
        }
    }
}
