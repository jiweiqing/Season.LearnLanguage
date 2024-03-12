using Learning.Domain;
using Microsoft.Extensions.DependencyInjection;
using Yitter.IdGenerator;

namespace FileService.Domain
{
    public class FileServiceDomainModule : IModuleInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            // 雪花id
            var options = new IdGeneratorOptions(2);
            YitIdHelper.SetIdGenerator(options);

            //var newId = YitIdHelper.NextId();
            services.AddScoped<FileDomainService>();
            services.AddScoped<CurrentUserContext>();
        }
    }
}
