using IdentityService.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly IdentityServiceDbContext _dbContext;
        public UserRepository(IdentityServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task<User?> GetUserByName(string userName)
        {
            return _dbContext.Users.Where(u => u.UserName == userName).SingleOrDefaultAsync();
        }

        public Task UpdateAsync(User user, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            _dbContext.Users.Update(user);
            return Task.CompletedTask;
        }
    }
}
