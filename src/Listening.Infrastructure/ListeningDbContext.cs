using Learning.Infrastructure;
using Listening.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listening.Infrastructure
{
    public class ListeningDbContext : DbContextBase
    {
        public ListeningDbContext(DbContextOptions options) : base(options)
        {
        }

        #region DbSet

        public DbSet<Category> Categories { get; set; }

        #endregion
    }
}
