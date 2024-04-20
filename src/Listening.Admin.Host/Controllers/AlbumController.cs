using AutoMapper;
using Learning.AspNetCore;
using Learning.Domain;
using Listening.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Listening.Admin.Host.Controllers
{
    /// <summary>
    /// 专辑相关
    /// </summary>
    public class AlbumController : ListeningControllerBase
    {
        private readonly IAlbumRepository _albumRepository;
        private readonly AlbumDomainService _albumDomainService;
        private readonly IMapper _mapper;
        public AlbumController(IAlbumRepository albumRepository, AlbumDomainService albumDomainService, IMapper mapper)
        {
            _albumRepository = albumRepository;
            _albumDomainService = albumDomainService;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取专辑列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<PagedResult<AlbumDto>> GetListAsync([FromQuery] GetAlbumsInput input)
        {
            var ablums = await _albumRepository.GetListAsync(input);
            int count = await _albumRepository.CountAsync(input);

            List<AlbumDto> dtos = _mapper.Map<List<AlbumDto>>(ablums);

            return new PagedResult<AlbumDto>
            {
                Items = dtos,
                TotalCount = count
            };
        }

        /// <summary>
        /// 查询指定专辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:long}")]
        [Authorize]
        public async Task<AlbumDto> GetAsync(long id)
        {
            var ablum = await _albumRepository.GetAsync(id);
            if (ablum == null)
            {
                throw new BusinessException("专辑不存在");
            }

            AlbumDto dto = _mapper.Map<AlbumDto>(ablum);
            return dto;
        }

        /// <summary>
        /// 创建专辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<AlbumDto>> CreateAsync(CreateAlbumDto input)
        {
            var album = await _albumDomainService.CreateAsync(input.Name, input.CategoryId, input.Description);
            var dto = _mapper.Map<AlbumDto>(album);
            return CreatedAtAction(nameof(GetAsync), new { id = dto.Id }, dto);
        }

        /// <summary>
        /// 编辑专辑
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="input">输入参数</param>
        /// <returns></returns>
        [HttpPut("{id:long}")]
        [Authorize]
        public async Task UpdateAsync(long id, UpdateAlbumDto input)
        {
            await _albumDomainService.UpdateAsync(id,input.Name,input.Description);
        }

        /// <summary>
        /// 删除专辑
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpDelete("{id:long}")]
        [Authorize]
        public async Task DeleteAsync(long id)
        {
            await _albumDomainService.DeleteAsync(id);
        }

        /// <summary>
        /// 获取专辑列表 —— 前台使用
        /// </summary>
        /// <param name="baseInput"></param>
        /// <returns></returns>
        [HttpGet("front")]
        public async Task<PagedResult<AlbumDto>> GetListByFrontAsync([FromQuery] GetAlbumsBaseInput baseInput)
        {
            GetAlbumsInput input = _mapper.Map<GetAlbumsInput>(baseInput);
            input.IsEabled = true;

            var ablums = await _albumRepository.GetListAsync(input);
            int count = await _albumRepository.CountAsync(input);

            List<AlbumDto> dtos = _mapper.Map<List<AlbumDto>>(ablums);

            return new PagedResult<AlbumDto>
            {
                Items = dtos,
                TotalCount = count
            };
        }

        /// <summary>
        /// 查询指定专辑 —— 前台使用
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpGet("{id:long}/front")]
        [Authorize]
        public async Task<AlbumDto> GetByFrontAsync(long id)
        {
            var ablum = await _albumRepository.GetAsync(id);
            if (ablum == null)
            {
                throw new BusinessException("专辑不存在");
            }

            AlbumDto dto = _mapper.Map<AlbumDto>(ablum);
            return dto;
        }
    }
}
