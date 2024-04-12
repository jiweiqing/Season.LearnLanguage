using Learning.Domain;
using Microsoft.Extensions.DependencyInjection;
using Yitter.IdGenerator;

namespace MediaEncoder.Domain
{
    public class MediaEncoderDomainModule : IModuleInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            var options = new IdGeneratorOptions(3);
            YitIdHelper.SetIdGenerator(options);

            services.AddScoped<MediaEncoderFactory>();
        }
    }
}
