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
        private readonly IStorageClinet _backupStorageClinet;
        private readonly IStorageClinet _remoteStorageClinet;
        public FileDomainService(
            IFileRepository fileRepository,
            IEnumerable<IStorageClinet> storageClinets)
        {
            _fileRepository = fileRepository;
            _backupStorageClinet = storageClinets.First(s => s.StorageType == StorageType.Backup);
            _remoteStorageClinet = storageClinets.First(s => s.StorageType == StorageType.Backup); ;
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

            // 备份服务器 速度快
            Uri backUrl = await _backupStorageClinet.SaveAsync(key, stream, cancellationToken);

            // 远程服务器
            stream.Position = 0;
            Uri remoteUrl = await _remoteStorageClinet.SaveAsync(key, stream, cancellationToken);

            stream.Position = 0;

            // todo 自增id
            long id = YitIdHelper.NextId();

            return FileUploadRecord.Create(id, fileSize, fileName, hash, backUrl.AbsoluteUri, remoteUrl.AbsoluteUri);
        }
    }
}
