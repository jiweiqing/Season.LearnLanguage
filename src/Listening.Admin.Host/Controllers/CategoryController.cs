using AutoMapper;
using Learning.AspNetCore;
using Learning.Domain;
using Listening.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Listening.Admin.Host
{
    /// <summary>
    /// 获取分类列表
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _repository;
        private readonly CategoryDomainService _domainService;
        private readonly IMapper _mapper;
        public CategoryController(
            ICategoryRepository repository,
            CategoryDomainService domainService,
            IMapper mapper)
        {
            _repository = repository;
            _domainService = domainService;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取分类列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedResult<CategoryDto>> GetListAsync([FromQuery] GetCategoriesInput input)
        {
            var count = await _repository.CountAsync(input);
            var categories = await _repository.GetListAsync(input);

            var dtos = _mapper.Map<List<CategoryDto>>(categories);

            PagedResult<CategoryDto> pagedResult = new PagedResult<CategoryDto>(dtos, count);

            return pagedResult;
        }

        /// <summary>
        /// 获取指定的分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:long}")]
        public async Task<CategoryDto> GetAsync(long id)
        {
            var category = await _repository.GetAsync(id);

            if (category == null)
            {
                throw new BusinessException("分类不存在");
            }

            var dto = _mapper.Map<CategoryDto>(category);
            return dto;
        }

        /// <summary>
        /// 创建分类
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CategoryDto>> CreateAsync(CreateCategoryDto createDto)
        {
            var category = await _domainService.CreateAsync(createDto.Name, createDto.ImageUrl);
            var dto = _mapper.Map<CategoryDto>(category);
            return CreatedAtAction(nameof(GetAsync), new { id = dto.Id }, dto);
        }

        /// <summary>
        /// 编辑分类
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="update"></param>
        /// <returns></returns>
        [HttpPut("{id:long}")]
        [Authorize(Roles = "Admin")]
        public async Task UpdateAsync(long id, UpdateCategoryDto update)
        {
            await _domainService.UpdateAsync(id, update.Name, update.ImageUrl);
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:long}")]
        [Authorize(Roles = "Admin")]
        public async Task DeleteAsync(long id)
        {
            await _domainService.DeleteAsync(id);
        }
    }
}
