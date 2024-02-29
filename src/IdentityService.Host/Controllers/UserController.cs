using AutoMapper;
using IdentityService.Domain;
using Microsoft.AspNetCore.Mvc;
using Learning.Domain;

namespace IdentityService.Host.Controllers
{
    /// <summary>
    /// 用户相关
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly UserDomainService _userDomainService;
        private readonly IMapper _mapper;
        public UserController(IUserRepository userRepository, UserDomainService userDomainService, IMapper mapper)
        {
            _userRepository = userRepository;
            _userDomainService = userDomainService;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedResult<UserDto>> GetListAsync([FromQuery] GetUsersInput input)
        {
            var repositoryInput = _mapper.Map<IncludesUsersInput>(input);

            var users = await _userRepository.GetListAsync(repositoryInput);

            var count = await _userRepository.CountAsync(repositoryInput);

            var dtos = _mapper.Map<List<UserDto>>(users);

            PagedResult<UserDto> pagedResult = new PagedResult<UserDto>(dtos, count);

            return pagedResult;
        }

        /// <summary>
        /// 获取指定用户
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        [HttpGet("{id:long}")]
        public async Task<UserDto> GetAsync(long id)
        {
            var user = await _userRepository.GetAsync(id, new IncludesUserDetailInput());
            if (user == null)
            {
                throw new BusinessException("target not found!");
            }

            var dto = _mapper.Map<UserDto>(user);

            return dto;
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="createDto">输入参数</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateAsync(CreateUserDto createDto)
        {
            var user = await _userDomainService.CreateUserAsync(createDto.UserName, createDto.NickName, createDto.Email, createDto.Password);

            var dto = _mapper.Map<UserDto>(user);

            return CreatedAtAction(nameof(GetAsync), new { id = dto.Id }, user);
        }

        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="updateDto">输入参数</param>
        /// <returns></returns>
        [HttpPut("{id:long}")]
        public async Task UpdateAsync(long id, UpdateUserDto updateDto)
        {
            await _userDomainService.UpdateUserAsync(id, updateDto.NickName, updateDto.Email);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:long}")]
        public async Task DeleteAsync(long id)
        {
            await _userRepository.DeleteAsync(id);
        }
    }
}
