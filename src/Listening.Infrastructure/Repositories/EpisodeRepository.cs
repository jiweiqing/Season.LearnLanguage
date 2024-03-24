using Learning.Infrastructure;
using Listening.Domain;
using Microsoft.EntityFrameworkCore;

namespace Listening.Infrastructure
{
    public class EpisodeRepository : Repository<long, Episode>, IEpisodeRepository
    {
        public EpisodeRepository(ListeningDbContext context) : base(context)
        {
        }

        public async Task<int> CountAsync(GetEpisodesInput input)
        {
            return await Build(DbSet, input).CountAsync();
        }

        public async Task<List<Episode>> GetListAsync(GetEpisodesInput input)
        {
            return await Build(DbSet, input, true).ToListAsync();
        }

        private IQueryable<Episode> Build(IQueryable<Episode> query, GetEpisodesInput input, bool paged = false)
        {
            query = query
               .WhereIf(!string.IsNullOrWhiteSpace(input.Name), c => c.Name.Contains(input.Name!))
               .WhereIf(input.SubtitleType.HasValue, c => c.SubtitleType == input.SubtitleType);

            if (paged)
            {
                query = query.OrderBy(c => c.CreationTime).Page(input);
            }
            return query;
        }
    }
}
