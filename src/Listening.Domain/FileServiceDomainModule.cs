using Learning.Domain;
using Microsoft.Extensions.DependencyInjection;
using Yitter.IdGenerator;

namespace Listening.Domain
{
    public class ListeningDomainModule : IModuleInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            // 雪花id
            var options = new IdGeneratorOptions(3);
            YitIdHelper.SetIdGenerator(options);

            //var newId = YitIdHelper.NextId();
            services.AddScoped<CurrentUserContext>();
            services.AddScoped<CategoryDomainService>();
            services.AddScoped<AlbumDomainService>();
            services.AddScoped<EpisodeDomainService>();

        }
    }
}
