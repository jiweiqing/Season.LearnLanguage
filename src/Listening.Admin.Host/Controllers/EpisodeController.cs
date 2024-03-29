//using AutoMapper;
//using Leaning.EventBus;
//using Learning.AspNetCore;
//using Learning.Domain;
//using Learning.Infrastructure;
//using Listening.Domain;
//using Microsoft.AspNetCore.Mvc;
//using System.Collections;
//using System.Collections.Generic;
//using Yitter.IdGenerator;

//namespace Listening.Admin.Host.Controllers
//{
//    /// <summary>
//    /// 音频相关
//    /// </summary>
//    [Route("api/[controller]")]
//    [ApiController]
//    public class EpisodeController : ControllerBase
//    {
//        private readonly IEpisodeRepository _episodeRepository;
//        private readonly EpisodeDomainService _episodeDomainService;
//        private readonly EncodingEpisodeHelper _encodingEpisodeHelper;
//        private readonly IEventBus _eventBus;
//        private readonly IMapper _mapper;
//        public EpisodeController(
//            IEpisodeRepository episodeRepository,
//            EncodingEpisodeHelper encodingEpisodeHelper,
//            EpisodeDomainService episodeDomainService,
//            IEventBus eventBus,
//            IMapper mapper)
//        {
//            _episodeRepository = episodeRepository;
//            _episodeDomainService = episodeDomainService;
//            _encodingEpisodeHelper = encodingEpisodeHelper;
//            _eventBus = eventBus;
//            _mapper = mapper;
//        }

//        /// <summary>
//        /// 获取音频列表
//        /// </summary>
//        /// <param name="input"></param>
//        /// <returns></returns>
//        [HttpGet]
//        public async Task<PagedResult<EpisodeDto>> GetListAsync(GetEpisodesInput input)
//        {
//            var count = await _episodeRepository.CountAsync(input);

//            var episodes = await _episodeRepository.GetListAsync(input);

//            var dtos = _mapper.Map<List<EpisodeDto>>(episodes);

//            PagedResult<EpisodeDto> pagedResult = new PagedResult<EpisodeDto>(dtos, count);

//            return pagedResult;
//        }

//        /// <summary>
//        /// 获取指定音频
//        /// </summary>
//        /// <param name="id">id</param>
//        /// <returns></returns>
//        [HttpGet("{id:long}")]
//        public async Task<EpisodeDto> GetAsync(long id)
//        {
//            var episode = await _episodeRepository.GetAsync(id);

//            if (episode == null)
//            {
//                throw new BusinessException("音频不存在!");
//            }

//            var dto = _mapper.Map<EpisodeDto>(episode);
//            return dto;
//        }

//        /// <summary>
//        /// 创建音频
//        /// </summary>
//        /// <param name="dto"></param>
//        /// <returns></returns>
//        [HttpPost]
//        public async Task<ActionResult<EpisodeDto>> CreateAsync(CreateEpisodeDto dto)
//        {
//            // 如果上传的是m4a文件,不用转码,直接存到数据库
//            if (dto.Resource.EndsWith("m4a", StringComparison.OrdinalIgnoreCase))
//            {
//                var episode = await _episodeDomainService.CreateAsync(dto.Name, dto.AlbumId, dto.Resource, dto.Duration, dto.Subtitle, dto.SubtitleType);
//                EpisodeDto episodeDto = _mapper.Map<EpisodeDto>(episode);
//                return CreatedAtAction(nameof(GetAsync), new { id = episode.Id }, episodeDto);
//            }
//            else
//            {
//                // 非m4a文件需要先转码,临时插入redis,转码完成再插入数据库
//                // 音频id
//                long id = YitIdHelper.NextId();
//                EncodingEpisodeInfo episodeInfo = new EncodingEpisodeInfo()
//                {
//                    Id = id,
//                    Name = dto.Name,
//                    AlbumId = dto.AlbumId,
//                    Resource = dto.Resource,
//                    Duration = dto.Duration,
//                    Subtitle = dto.Subtitle,
//                    SubtitleType = dto.SubtitleType,
//                    Status = "Created"
//                };

//                await _encodingEpisodeHelper.AddEncodingEpisodeAsync(id, episodeInfo);
//                // TODO:通知转码服务,启动转码. 转码服务未实现
//                _eventBus.Publish("MediaEncoding.Created", new { MediaId = id, MediaUrl = episodeInfo.Resource, OutputFormat = "m4a", SourceSystem = "Listening" });
//                return Ok();
//            }
//        }

//        /// <summary>
//        /// 编辑音频
//        /// </summary>
//        /// <param name="id"></param>
//        /// <param name="updateDto"></param>
//        /// <returns></returns>
//        [HttpPut("{id:long}")]
//        public async Task UpdateAsync(long id, UpdateEpisodeDto updateDto)
//        {
//            await _episodeDomainService.UpdateAsync(id, updateDto.Name, updateDto.Subtitle, updateDto.SubtitleType);
//        }

//        /// <summary>
//        /// 删除音频
//        /// </summary>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        [HttpDelete("{id:long}")]
//        public async Task DeleteAsync(long id)
//        {
//            await _episodeDomainService.DeleteAsync(id);
//        }

//        /// <summary>
//        /// 获取所有指定专辑下的转码任务
//        /// </summary>
//        /// <param name="albumId"></param>
//        /// <returns></returns>
//        [HttpGet("{albumId:long}/encoding")]
//        public async Task<List<EncodingEpisodeInfo>> GetEncodingEpisodesByAlbumIdAsync(long albumId)
//        {
//            List<EncodingEpisodeInfo> episodeInfos  = new List<EncodingEpisodeInfo>();
//            var episodeIds = await _encodingEpisodeHelper.GetEncodingEpisodeIdsAsync(albumId);
//            foreach (long episodeId in episodeIds)
//            {
//                var encodingEpisode = await _encodingEpisodeHelper.GetEncodingEpisodeAsync(episodeId);
//                if (!string.Equals(encodingEpisode.Status,"Completed",StringComparison.OrdinalIgnoreCase) )//不显示已经完成的
//                {
//                    episodeInfos.Add(encodingEpisode);
//                }
//            }
//            return episodeInfos;
//        }

//        /// <summary>
//        /// 启用
//        /// </summary>
//        /// <param name="id">id</param>
//        /// <returns></returns>
//        /// <exception cref="BusinessException"></exception>
//        [HttpPut]
//        [Route("{id}/enable")]
//        public async Task EnableAsync(long id)
//        {
//            var episode = await _episodeRepository.GetAsync(id);
//            if (episode == null)
//            {
//                throw new BusinessException("音频不存在");
//            }
//            episode.Hide();
//        }

//        /// <summary>
//        /// 禁用
//        /// </summary>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        /// <exception cref="BusinessException"></exception>
//        [HttpPut]
//        [Route("{id}/disable")]
//        public async Task DisableAsync(long id)
//        {
//            var episode = await _episodeRepository.GetAsync(id);
//            if (episode == null)
//            {
//                throw new BusinessException("音频不存在");
//            }
//            episode.Show();
//        }

//        // TODO:排序接口
//    }
//}
