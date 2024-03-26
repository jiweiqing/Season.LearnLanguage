using Learning.Infrastructure;
using Listening.Domain;
using StackExchange.Redis;

namespace Listening.Admin.Host
{
    public class EncodingEpisodeHelper
    {
        private readonly IConnectionMultiplexer _redisConnection;
        public EncodingEpisodeHelper(IConnectionMultiplexer redisConnection)
        {
            _redisConnection = redisConnection;
        }

        public async Task AddEncodingEpisodeAsync(long episodeId, EncodingEpisodeInfo episodeInfo)
        {
            string episodeKey = GetEpisodeStatusKey(episodeId);
            var db = _redisConnection.GetDatabase();
            // 将要转码的数据存入redis
            await db.StringSetAsync(episodeKey, episodeInfo.ToJsonString());
            string ablumKey = GetEncodingAlbumKey(episodeInfo.AlbumId);
            // redis Set
            await db.SetAddAsync(ablumKey, episodeId.ToString());
        }

        /// <summary>
        /// 获取albumId(专辑)下的所有转码任务
        /// </summary>
        /// <param name="albumId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<long>> GetEncodingEpisodeIdsAsync(long albumId)
        {
            string ablumKey = GetEncodingAlbumKey(albumId);
            var db = _redisConnection.GetDatabase();
            var values = await db.SetMembersAsync(ablumKey);
            return values.Select(x => (long)x);
        }

        /// <summary>
        /// 删除一个Episode转码的任务
        /// </summary>
        /// <param name="episodeId"></param>
        /// <param name="albumId"></param>
        /// <returns></returns>
        public async Task RemoveEncodingEpisodeAsync(long episodeId,long albumId) 
        {
            string episodeKey = GetEpisodeStatusKey(episodeId);
            var db = _redisConnection.GetDatabase();
            await db.KeyDeleteAsync(episodeKey);
            string ablumKey = GetEncodingAlbumKey(albumId);
            await db.SetRemoveAsync(ablumKey, episodeId.ToString());
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="episodeId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task UpdateEpisodeStatusAsync(long episodeId, string status)
        {
            string episodeKey = GetEpisodeStatusKey(episodeId);
            var db = _redisConnection.GetDatabase();
            string json = await db.StringGetAsync(episodeKey);
            EncodingEpisodeInfo? episodeInfo = json.Deserialize<EncodingEpisodeInfo>();
            if (episodeInfo != null)
            {
                episodeInfo.Status = status;
                await db.StringSetAsync(episodeKey, episodeInfo.ToJsonString());
            }
        }

        /// <summary>
        /// 获得Episode的转码状态
        /// </summary>
        /// <param name="db"></param>
        /// <param name="episodeId"></param>
        /// <returns></returns>
        public async Task<EncodingEpisodeInfo> GetEncodingEpisodeAsync(long episodeId)
        {
            string episodeKey = GetEpisodeStatusKey(episodeId);
            var db = _redisConnection.GetDatabase();
            string json = await db.StringGetAsync(episodeKey);
            EncodingEpisodeInfo episode = json.Deserialize<EncodingEpisodeInfo>()!;
            return episode;
        }

        #region private methods
        /// <summary>
        /// 获取音频redis key
        /// </summary>
        /// <param name="episodeId"></param>
        /// <returns></returns>
        private static string GetEpisodeStatusKey(long episodeId)
        {
            string redisKey = $"Listening.EncodingEpisode.{episodeId}";
            return redisKey;
        }

        /// <summary>
        /// 获取专辑 redis key
        /// </summary>
        /// <param name="albumId"></param>
        /// <returns></returns>
        private static string GetEncodingAlbumKey(long albumId)
        {
            return $"Listening.EncodingAblum.{albumId}";
        }

        #endregion
    }
}
