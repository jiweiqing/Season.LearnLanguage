using Learning.Domain;

namespace Listening.Domain
{
    public class EpisodeDomainService
    {
        private readonly IEpisodeRepository _episodeRepository;
        public EpisodeDomainService(IEpisodeRepository episodeRepository)
        {
            _episodeRepository = episodeRepository;
        }

        /// <summary>
        /// 创建音频
        /// </summary>
        /// <param name="name"></param>
        /// <param name="albumId"></param>
        /// <param name="resource"></param>
        /// <param name="duration"></param>
        /// <param name="subtitle"></param>
        /// <param name="subtitleType"></param>
        /// <returns></returns>
        public async Task<Episode> CreateAsync(
            string name, long albumId,
            string resource, double duration,
            string subtitle, SubtitleType subtitleType)
        {
            var maxOrder = await _episodeRepository.GetMaxSortOrderAsync();
            Episode episode = Episode.Create(maxOrder, name, albumId, resource, duration, subtitle, subtitleType);
            await _episodeRepository.InsertAsync(episode);
            return episode;
        }

        /// <summary>
        /// 更新音频
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="resource"></param>
        /// <param name="duration"></param>
        /// <param name="subtitle"></param>
        /// <param name="subtitleType"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<Episode> UpdateAsync(
            long id, string name,
            string resource, double duration,
            string subtitle, SubtitleType subtitleType)
        {
            var episode = await _episodeRepository.GetAsync(id);
            if (episode == null)
            {
                throw new BusinessException("专辑不存在");
            }
            episode.Update(name, resource, duration, subtitle, subtitleType);
            return episode;
        }

        /// <summary>
        /// 删除音频
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(long id)
        {
            await _episodeRepository.DeleteAsync(id);

            // TODO:发送事件,删除服务器上的音频
        }
    }
}
