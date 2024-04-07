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
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), c => c.Name.Contains(input.Name!))
                .WhereIf(input.IsEabled.HasValue, c => c.IsEabled == input.IsEabled)
                .WhereIf(input.CategoryId.HasValue, c => c.CategoryId == input.CategoryId);

            if (paged)
            {
                query = query.OrderBy(c => c.CreationTime).Page(input);
            }
            return query;
        }

        public async Task<int> GetMaxSortOrderAsync()
        {
            return await DbSet.OrderByDescending(c => c.SortOrder).Select(c => c.SortOrder).FirstOrDefaultAsync();
        }
    }
}
