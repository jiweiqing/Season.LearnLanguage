using MediatR;

namespace Listening.Domain.Events
{
    /// <summary>
    /// 删除音频事件
    /// </summary>
    /// <param name="id"></param>
    public class EpisodeDeletedEvent(long id): INotification
    {
        public long Id { get; } = id;
    }
}
