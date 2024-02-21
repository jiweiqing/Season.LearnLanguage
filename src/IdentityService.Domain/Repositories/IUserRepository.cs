namespace IdentityService.Domain
{
    public interface IUserRepository : IRepository<long,User>, IScopedDependency
    {
        /// <summary>
        /// 依据用户名查询用户
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<User?> GetUserByName(string userName);

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<User>> GetListAsync(GetUsersInput input);
    }
}
