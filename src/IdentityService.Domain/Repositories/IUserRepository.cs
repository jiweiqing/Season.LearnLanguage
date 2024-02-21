namespace IdentityService.Domain
{
    public interface IUserRepository : IRepository<long,User>, IScopedDependency
    {
        Task<User?> GetUserByName(string userName);
    }
}
