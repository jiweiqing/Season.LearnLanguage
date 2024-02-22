using IdentityService.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<List<User>> GetListAsync(GetUsersInput input)
        {
            var query = Build(DbSet,input);

            query = query.OrderByDescending(u => u.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount);
            return await query.ToListAsync();
        }

        public async Task<int> CountAsync(GetUsersInput input)
        {
            var query = DbSet
                .WhereIf(!string.IsNullOrWhiteSpace(input.UserName), u => u.UserName.Contains(input.UserName!))
                .WhereIf(!string.IsNullOrWhiteSpace(input.NickName), u => u.NickName.Contains(input.NickName!))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Email), u => u.Email == input.Email);

            return await query.CountAsync();
        }

        private IQueryable<User> Build(IQueryable<User> query, GetUsersInput input)
        {
            query = query
                .WhereIf(!string.IsNullOrWhiteSpace(input.UserName), u => u.UserName.Contains(input.UserName!))
                .WhereIf(!string.IsNullOrWhiteSpace(input.NickName), u => u.NickName.Contains(input.NickName!))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Email), u => u.Email == input.Email);

            return query;
        }
    }
}
