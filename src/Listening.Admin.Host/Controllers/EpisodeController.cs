using AutoMapper;
using Learning.AspNetCore;
using Learning.Domain;
using Listening.Domain;
using Microsoft.AspNetCore.Mvc;
using Yitter.IdGenerator;

namespace Listening.Admin.Host.Controllers
{
    /// <summary>
    /// 音频相关
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EpisodeController : ControllerBase
    {
        private readonly IEpisodeRepository _episodeRepository;
        private readonly EpisodeDomainService _episodeDomainService;
        private readonly IMapper _mapper;
        public EpisodeController(
            IEpisodeRepository episodeRepository, 
            EpisodeDomainService episodeDomainService, 
            IMapper mapper)
        {
            _episodeRepository = episodeRepository;
            _episodeDomainService = episodeDomainService;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取音频列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedResult<EpisodeDto>> GetListAsync(GetEpisodesInput input)
        {
            var count = await _episodeRepository.CountAsync(input);

            var episodes = await _episodeRepository.GetListAsync(input);

            var dtos = _mapper.Map<List<EpisodeDto>>(episodes);

            PagedResult<EpisodeDto> pagedResult = new PagedResult<EpisodeDto>(dtos, count);

            return pagedResult;
        }

        /// <summary>
        /// 获取指定音频
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpGet("{id:long}")]
        public async Task<EpisodeDto> GetAsync(long id)
        {
            var episode = await _episodeRepository.GetAsync(id);

            if (episode == null)
            {
                throw new BusinessException("音频不存在!");
            }

            var dto = _mapper.Map<EpisodeDto>(episode);
            return dto;
        }

        public async Task<EpisodeDto> CreateAsync(CreateEpisodeDto dto)
        {
            // 如果上传的是m4a文件,不用转码,直接存到数据库
            if (dto.Resource.EndsWith("m4a", StringComparison.OrdinalIgnoreCase))
            {
                await _episodeDomainService.CreateAsync(dto.Name, dto.AlbumId, dto.Resource, dto.Duration, dto.Subtitle, dto.SubtitleType);
            }
            else
            {
                // 非m4a文件需要先转码,临时插入redis,转码完成再插入数据库
                // 音频id
                long id = YitIdHelper.NextId();
                EncodingEpisodeInfo episodeInfo = new EncodingEpisodeInfo()
                {
                    Id = id,
                    Name = dto.Name,
                    AlbumId = dto.AlbumId,
                    Resource = dto.Resource,
                    Duration = dto.Duration,
                    Subtitle = dto.Subtitle,
                    SubtitleType = dto.SubtitleType,
                    Status = "Created"
                };

                // TODO:
                throw new NotImplementedException();
            }
            throw new NotImplementedException();

        }
    }
}
