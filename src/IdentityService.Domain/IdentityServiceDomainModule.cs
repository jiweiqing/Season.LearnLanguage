using Learning.Domain;
using Microsoft.Extensions.DependencyInjection;
using Yitter.IdGenerator;

namespace IdentityService.Domain
{
    public class IdentityServiceDomainModule : IModuleInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            // 雪花id
            var options = new IdGeneratorOptions(1);
            YitIdHelper.SetIdGenerator(options);

            //var newId = YitIdHelper.NextId();
            services.AddScoped<UserDomainService>();
            services.AddScoped<CurrentUserContext>();
        }
    }
}
