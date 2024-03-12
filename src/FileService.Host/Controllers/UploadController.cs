using FileService.Domain;
using FileService.Host;
using FileService.Host.Dtos;
using FileService.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Host
{
    /// <summary>
    /// 账号相关
    /// </summary>
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly FileServiceDbContext _dbContext;
        private readonly FileDomainService _domainService;
        private readonly IFileRepository _fileRepository;
        public UploadController(
            FileServiceDbContext dbContext,
            FileDomainService domainService,
            IFileRepository repository) 
        {
            _dbContext = dbContext;
            _domainService = domainService;
            _fileRepository = repository;
        }

        
        /// <summary>
        /// 检查文件是否已存在
        /// </summary>
        /// <param name="fileSize">文件大小</param>
        /// <param name="sha256Hash">hash</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<FileExistsDto> FileExists(long fileSize, string sha256Hash)
        {
            var item = await _fileRepository.FindFileAsync(fileSize, sha256Hash);
            if (item == null)
            {
                return new FileExistsDto();
            }
            else
            {
                return new FileExistsDto()
                {
                    IsExists = true,
                    Url = item.RemoteUrl
                };
            }
        }

        [HttpPost]
        public async Task<UploadResultDto> Upload(IFormFile file, CancellationToken cancellationToken = default)
        {
            string fileName = file.FileName;
            using Stream stream = file.OpenReadStream();
            var record = await _domainService.UploadAsync(stream, fileName, cancellationToken);
            _dbContext.Add(record);
            UploadResultDto result = new UploadResultDto()
            {
                Url = record.RemoteUrl
            };
            return result;
        }

    }
}
