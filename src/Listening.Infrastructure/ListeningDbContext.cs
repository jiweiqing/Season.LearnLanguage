using Learning.Infrastructure;
using Listening.Domain;
using Microsoft.EntityFrameworkCore;

namespace Listening.Infrastructure
{
    public class ListeningDbContext : DbContextBase
    {
        public ListeningDbContext(DbContextOptions options) : base(options)
        {
        }

        #region DbSet

        public DbSet<Category> Categories { get; set; }
        public DbSet<Album> Albums  { get; set; }

        #endregion
    }
}
