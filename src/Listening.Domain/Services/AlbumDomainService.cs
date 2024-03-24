using Learning.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Listening.Domain
{
    public class AlbumDomainService
    {
        private readonly IAlbumRepository _albumRepository;
        public AlbumDomainService(IAlbumRepository albumRepository)
        {
            _albumRepository = albumRepository;
        }

        public async Task<Album> CreateAsync(int sortOrder, string name, long categoryId, string? description)
        {
            Album album = Album.Create(sortOrder, name, categoryId, description);
            await _albumRepository.InsertAsync(album);
            return album;
        }

        public async Task<Album> UpdateAsync(long id, int sortOrder, string name, long categoryId, string? description)
        {
            var ablum = await _albumRepository.GetAsync(id);
            if (ablum == null)
            {
                throw new BusinessException("专辑不存在");
            }
            ablum.Update(sortOrder, name, categoryId, description); 
            return ablum;
        }

        public async Task DeleteAsync(long id)
        {
            // TODO: 判断专辑下是否有音频
            await _albumRepository.DeleteAsync(id);
        }
    }
}
