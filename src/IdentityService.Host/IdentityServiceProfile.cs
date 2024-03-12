using AutoMapper;
using IdentityService.Domain;

namespace IdentityService.Host
{
    public class IdentityServiceProfile : Profile
    {
        public IdentityServiceProfile()
        {
            #region user

            CreateMap<GetUsersInput, IncludesUsersInput>();
            CreateMap<User, UserDto>();

            #endregion

        }
    }
}
