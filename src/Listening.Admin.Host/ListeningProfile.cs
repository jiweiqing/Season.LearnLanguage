using AutoMapper;
using Listening.Domain;

namespace Listening.Admin.Host
{
    public class ListeningProfile: Profile
    {
        public ListeningProfile() 
        {
            CreateMap<Category, CategoryDto>();
        }
    }
}
