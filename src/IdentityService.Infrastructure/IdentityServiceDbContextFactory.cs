using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace IdentityService.Infrastructure
{
    public class IdentityServiceDbContextFactory : IDesignTimeDbContextFactory<IdentityServiceDbContext>
    {
        public IdentityServiceDbContext CreateDbContext(string[] args)
        {
            string hostPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "IdentityService.Host");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(hostPath)
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile("appsettings.Development.json", true)
                .Build();

            var optionsBulider = new DbContextOptionsBuilder<IdentityServiceDbContext>();
            optionsBulider.UseMySql(configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(configuration["Mysql:Version"]));

            //optionsBulider.UseMySql("Server=localhost;Port=3306;Database=seasonapp;Uid=root;Pwd=async_await;", new MySqlServerVersion(configuration["Mysql:Version"]));

            return new IdentityServiceDbContext(optionsBulider.Options);
        }
    }
}
