using AutoMapper;
using Listening.Domain;

namespace Listening.Admin.Host
{
    public class ListeningProfile: Profile
    {
        public ListeningProfile() 
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<Album, AlbumDto>();
            CreateMap<Episode, EpisodeDto>();
            CreateMap<Episode, EpisodeFrontDto>();

            CreateMap<GetAlbumsBaseInput, GetAlbumsInput>();
        }
    }
}
