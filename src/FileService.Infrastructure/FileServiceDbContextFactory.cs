using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService.Infrastructure
{
    public class FileServiceDbContextFactory : IDesignTimeDbContextFactory<FileServiceDbContext>
    {
        public FileServiceDbContext CreateDbContext(string[] args)
        {
            string hostPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "IdentityService.Host");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(hostPath)
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile("appsettings.Development.json", true)
                .Build();

            var optionsBulider = new DbContextOptionsBuilder<FileServiceDbContext>();
            optionsBulider.UseMySql(configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(configuration["Mysql:Version"]));

            return new FileServiceDbContext(optionsBulider.Options);
        }
    }
}
