using IdentityService.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yitter.IdGenerator;

namespace IdentityService.Infrastructure
{
    public class IdentitySeederService: IIdentitySeederService
    {
        private readonly IdentityServiceDbContext _dbContext;
        public IdentitySeederService(IdentityServiceDbContext dbContext) 
        {
            _dbContext = dbContext;
        }
        public async Task SeedAsync()
        {
            await InitRolesAsync();
            await InitUserAsync();
            await _dbContext.SaveChangesAsync();

        }

        private async Task InitRolesAsync()
        {
            if (await _dbContext.Roles.AnyAsync())
            {
                return;
            }

            Role role = new Role(YitIdHelper.NextId())
            {
                Name = "管理员",
                Code = "Admin"
            };

            //role.RolePermissions = new List<RolePermission>()
            //{
            //    new RolePermission(role.Id,UserManagement.GroupName),
            //    new RolePermission(role.Id,UserManagement.Query),
            //    new RolePermission(role.Id,UserManagement.Create),
            //    new RolePermission(role.Id,UserManagement.Update),
            //    new RolePermission(role.Id,UserManagement.Delete),
            //    new RolePermission(role.Id,UserManagement.ChangeUserRole),
            //    new RolePermission(role.Id,RoleManagement.GroupName),
            //    new RolePermission(role.Id,RoleManagement.Query),
            //    new RolePermission(role.Id,RoleManagement.Create),
            //    new RolePermission(role.Id,RoleManagement.Update),
            //    new RolePermission(role.Id,RoleManagement.Delete),
            //    new RolePermission(role.Id,RoleManagement.ChangeRolePermission)
            //};

            await _dbContext.AddAsync(role);
        }

        private async Task InitUserAsync()
        {
            if (await _dbContext.Users.AnyAsync())
            {
                return;
            }

            User user = new User(YitIdHelper.NextId());
            user.UserName = "admin";
            user.NickName = "admin";
            user.Password = EncryptHelper.MD5Encrypt("123456");
            user.Email = "jiweiqing7@hotmail.com";

            var role = await _dbContext.Roles.Where(r => r.Code == "Admin").FirstAsync();

            user.UserRoles = new List<UserRole>()
            {
                new UserRole(user.Id,role.Id)
            };

            await _dbContext.Users.AddAsync(user);
        }
    }
}
