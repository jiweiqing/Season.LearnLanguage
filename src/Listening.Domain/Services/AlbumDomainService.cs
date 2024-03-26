using Learning.Domain;

namespace Listening.Domain
{
    public class AlbumDomainService
    {
        private readonly IAlbumRepository _albumRepository;
        public AlbumDomainService(IAlbumRepository albumRepository)
        {
            _albumRepository = albumRepository;
        }

        public async Task<Album> CreateAsync(string name, long categoryId, string? description)
        {
            int maxOrder = await _albumRepository.GetMaxSortOrderAsync();
            Album album = Album.Create(maxOrder, name, categoryId, description);
            await _albumRepository.InsertAsync(album);
            return album;
        }

        public async Task<Album> UpdateAsync(long id,string name,string? description)
        {
            var ablum = await _albumRepository.GetAsync(id);
            if (ablum == null)
            {
                throw new BusinessException("专辑不存在");
            }
            ablum.Update(name, description); 
            return ablum;
        }

        public async Task DeleteAsync(long id)
        {
            // TODO: 判断专辑下是否有音频
            await _albumRepository.DeleteAsync(id);
        }
    }
}
