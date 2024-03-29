using Leaning.EventBus;
using Listening.Domain;
using MediatR;

namespace Listening.Admin.Host
{
    public class EpisodeCreatedEventHandler : INotificationHandler<EpisodeCreatedEvent>
    {
        private readonly IEventBus _eventBus;
        public EpisodeCreatedEventHandler(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }
        public Task Handle(EpisodeCreatedEvent notification, CancellationToken cancellationToken)
        {
            // 把领域事件转发为集成事件,让其他微服务监听到
            //在领域事件处理中集中进行更新缓存等处理，而不是写到Controller中。因为项目中有可能不止一个地方操作领域对象，这样就就统一了操作。
            var episode = notification.Episode;
            var sentences = episode.ParseSubtitle();
            // TODO:需要将事件名,写成常量或者枚举
            _eventBus.Publish("ListeningEpisode.Created",new 
            { 
                Id = episode.Id,
                Name = episode.Name,
                Sentences = sentences,
                AlbumId = episode.AlbumId,
                Subtitle = episode.Subtitle,
                SubtitleType = episode.SubtitleType.ToString()
            });
            return Task.CompletedTask;
        }
    }
}
