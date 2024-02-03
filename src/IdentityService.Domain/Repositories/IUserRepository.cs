using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Domain
{
    public interface IUserRepository
    {
        Task<User?> GetUserByName(string userName, string password);
        Task<User> UpdateAsync(User user,
            bool autoSave = false,
            CancellationToken cancellationToken = default);
    }
}
