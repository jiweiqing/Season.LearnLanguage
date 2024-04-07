using Learning.Infrastructure;
using Listening.Domain;
using Microsoft.EntityFrameworkCore;

namespace Listening.Infrastructure
{
    public class CategoryRepository : Repository<long, Category>, ICategoryRepository
    {
        public CategoryRepository(ListeningDbContext context) : base(context)
        {
        }

        public async Task<int> CountAsync(GetCategoriesInput input)
        {
            return await Build(DbSet, input).CountAsync();
        }

        public async Task<List<Category>> GetListAsync(GetCategoriesInput input)
        {
            return await Build(DbSet, input, true).ToListAsync();
        }

        public async Task<int> GetMaxSortOrderAsync()
        {
            return await DbSet.OrderByDescending(c => c.SortOrder).Select(c => c.SortOrder).FirstOrDefaultAsync();
        }

        private IQueryable<Category> Build(IQueryable<Category> query, GetCategoriesInput input, bool paged = false)
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
