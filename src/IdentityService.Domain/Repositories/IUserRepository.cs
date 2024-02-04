namespace IdentityService.Domain
{
    public interface IUserRepository : IScopedDependency
    {
        Task<User?> GetUserByName(string userName);
        Task UpdateAsync(User user,
            bool autoSave = false,
            CancellationToken cancellationToken = default);
    }
}
