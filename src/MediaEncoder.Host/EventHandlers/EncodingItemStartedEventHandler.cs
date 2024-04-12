using Leaning.EventBus;
using MediaEncoder.Domain;
using MediatR;

namespace MediaEncoder.Host
{
    public class EncodingItemStartedEventHandler : INotificationHandler<EncodingItemStartedEvent>
    {
        private readonly IEventBus eventBus;

        public EncodingItemStartedEventHandler(IEventBus eventBus)
        {
            this.eventBus = eventBus;
        }
        public Task Handle(EncodingItemStartedEvent notification, CancellationToken cancellationToken)
        {
            eventBus.Publish("MediaEncoding.Started", notification);
            return Task.CompletedTask;
        }
    }
}
