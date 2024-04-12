using Learning.Infrastructure;
using MediaEncoder.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaEncoder.Infrastructure
{
    public class MediaDbContext : DbContextBase
    {
        public MediaDbContext(DbContextOptions options) : base(options)
        {
        }

        #region DbSet

        public DbSet<EncodingItem> EncodingItems { get; set; }

        #endregion
    }
}
