using IdentityService.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Infrastructure
{
    public class UserRepository : Repository<long,User>, IUserRepository
    {
        private readonly IdentityServiceDbContext _dbContext;
        public UserRepository(IdentityServiceDbContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
        }
        public Task<User?> GetUserByName(string userName)
        {
            return _dbContext.Users.Where(u => u.UserName == userName).SingleOrDefaultAsync();
        }
    }
}
