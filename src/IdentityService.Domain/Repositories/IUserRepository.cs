using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Domain
{
    public interface IUserRepository : IScopedDependency
    {
        Task<User?> GetUserByName(string userName);
        Task UpdateAsync(User user,
            bool autoSave = false,
            CancellationToken cancellationToken = default);
    }
}
