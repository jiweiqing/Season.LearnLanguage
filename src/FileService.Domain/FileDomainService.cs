using Learning.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yitter.IdGenerator;

namespace FileService.Domain
{
    public class FileDomainService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IStorageClient _backupStorageClinet;
        private readonly IStorageClient _remoteStorageClinet;
        public FileDomainService(
            IFileRepository fileRepository,
            IEnumerable<IStorageClient> storageClinets)
        {
            _fileRepository = fileRepository;
            _backupStorageClinet = storageClinets.First(s => s.StorageType == StorageType.Backup);
            _remoteStorageClinet = storageClinets.First(s => s.StorageType == StorageType.Publich); ;
        }

        public async Task<FileUploadRecord> UploadAsync(Stream stream,string fileName,CancellationToken cancellationToken = default) 
        {
            string hash = HashHelper.ComputeSha256Hash(stream);
            long fileSize = stream.Length;
            DateTime today = DateTime.Today;
            // 文件保存目录
            string key = $"{today.Year}/{today.Month}/{today.Day}/{hash}/{fileName}";
            var oldFileRecord = await _fileRepository.FindFileAsync(fileSize, hash);
            if (oldFileRecord != null) 
            {
                return oldFileRecord;
            }

            stream.Position = 0;

            // 远程服务器
            //string backUrl = await _backupStorageClinet.SaveAsync(key, stream, cancellationToken);

            // 备份服务器
            stream.Position = 0;
            string remoteUrl = await _backupStorageClinet.SaveAsync(key, stream, cancellationToken);

            stream.Position = 0;

            long id = YitIdHelper.NextId();

            return FileUploadRecord.Create(id, fileSize, fileName, hash, null, remoteUrl);
        }
    }
}
