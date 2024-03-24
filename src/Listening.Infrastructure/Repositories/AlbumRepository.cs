using Learning.Infrastructure;
using Listening.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Listening.Infrastructure
{
    public class AlbumRepository : Repository<long, Album>, IAlbumRepository
    {
        public AlbumRepository(ListeningDbContext context) : base(context)
        {
        }

        public async Task<List<Album>> GetListAsync(GetAlbumsInput input)
        {
            return await Build(DbSet, input, true).ToListAsync();
        }

        public async Task<int> CountAsync(GetAlbumsInput input)
        {
            return await Build(DbSet, input).CountAsync();
        }
       

        public IQueryable<Album> Build(IQueryable<Album> query, GetAlbumsInput input, bool paged = false)
        {
            query = query
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), c => c.Name.Contains(input.Name!));

            if (paged)
            {
                query = query.OrderBy(c => c.CreationTime).Page(input);
            }
            return query;
        }
    }
}
