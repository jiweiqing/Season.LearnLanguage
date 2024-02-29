using Learning.Domain;

namespace IdentityService.Domain
{
    public interface IIdentitySeederService: IScopedDependency
    {
        Task SeedAsync();
    }
}
