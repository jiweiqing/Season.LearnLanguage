using FileService.Domain;
using Learning.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace FileService.Infrastructure
{
    public class FileServiceInfrastructureModule : IModuleInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddScoped<IStorageClient, LocalStorageClient>();
            services.AddScoped<IStorageClient, MockCloudStorageClient>();
            services.AddScoped<IFileRepository, FileRepository>();
        }
    }
}
