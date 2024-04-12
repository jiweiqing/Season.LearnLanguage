using MediaEncoder.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Listening.Infrastructure
{
    public class MediaDbContextFactory : IDesignTimeDbContextFactory<MediaDbContext>
    {
        public MediaDbContext CreateDbContext(string[] args)
        {
            string hostPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Listening.Admin.Host");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(hostPath)
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile("appsettings.Development.json", true)
                .Build();

            var optionsBulider = new DbContextOptionsBuilder<MediaDbContext>();
            optionsBulider.UseMySql(configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(configuration["Mysql:Version"]));

            return new MediaDbContext(optionsBulider.Options);
        }
    }
}
