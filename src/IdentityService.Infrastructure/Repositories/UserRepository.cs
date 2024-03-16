using IdentityService.Domain;
using Learning.Domain;
using Learning.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure
{
    public class UserRepository : Repository<long, User>, IUserRepository
    {
        private readonly IdentityServiceDbContext _dbContext;
        public UserRepository(IdentityServiceDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<User?> GetUserByName(string userName)
        {
            return _dbContext.Users.Where(u => u.UserName == userName).SingleOrDefaultAsync();
        }

        public Task<User?> GetUserByEmailAsync(string email)
        {
            return _dbContext.Users.Where(u => u.Email == email).SingleOrDefaultAsync();
        }

        public async Task<List<User>> GetListAsync(IncludesUsersInput input)
        {
            var query = Build(DbSet, input,true);

            return await query.ToListAsync();
        }

        public async Task<int> CountAsync(IncludesUsersInput input)
        {
            var query = Build(DbSet, input);
            return await query.CountAsync();
        }

        public async Task<User?> GetAsync(long id, IncludesUserDetailInput input)
        {
            var query =  _dbContext.Users.AsQueryable<User>();
            if (input.IncludeUserRole)
            {
                query = query.Include(c => c.UserRoles).ThenInclude(ur => ur.Role);
            }
            return await query.Where(u => u.Id == id).FirstOrDefaultAsync();
        }
        
        /// <summary>
        /// 构建查询表达式
        /// </summary>
        /// <param name="query"></param>
        /// <param name="input"></param>
        /// <param name="paged"></param>
        /// <returns></returns>
        private IQueryable<User> Build(IQueryable<User> query, IncludesUsersInput input, bool paged = false)
        {
            query = query
                .WhereIf(!string.IsNullOrWhiteSpace(input.UserName), u => u.UserName.Contains(input.UserName!))
                .WhereIf(!string.IsNullOrWhiteSpace(input.NickName), u => u.NickName.Contains(input.NickName!))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Email), u => u.Email == input.Email);

            if (input.IncludeUserRole)
            {
                query = query.Include(c => c.UserRoles).ThenInclude(ur => ur.Role);
            }

            if (paged)
            {
                query = query.OrderBy(c => c.CreationTime).Page(input);
            }

            return query;
        }
    }
}
