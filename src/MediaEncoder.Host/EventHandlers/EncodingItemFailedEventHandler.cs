using Leaning.EventBus;
using MediaEncoder.Domain;
using MediatR;

namespace MediaEncoder.Host
{
    public class EncodingItemFailedEventHandler : INotificationHandler<EncodingItemFailedEvent>
    {
        private readonly IEventBus eventBus;

        public EncodingItemFailedEventHandler(IEventBus eventBus)
        {
            this.eventBus = eventBus;
        }
        public Task Handle(EncodingItemFailedEvent notification, CancellationToken cancellationToken)
        {
            eventBus.Publish("MediaEncoding.Failed", notification);
            return Task.CompletedTask;
        }
    }
}
