using Leaning.EventBus;
using Listening.Domain;
using MediatR;

namespace Listening.Admin.Host
{
    public class EpisodeDeletedEventHandler : INotificationHandler<EpisodeDeletedEvent>
    {
        private readonly IEventBus _eventBus;
        public EpisodeDeletedEventHandler(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }
        public Task Handle(EpisodeDeletedEvent notification, CancellationToken cancellationToken)
        {
            var id = notification.Id;
            _eventBus.Publish("ListeningEpisode.Deleted", new { Id = id });
            return Task.CompletedTask;
        }
    }
}
