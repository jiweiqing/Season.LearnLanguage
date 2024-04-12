using Leaning.EventBus;
using MediaEncoder.Domain;
using MediatR;

namespace MediaEncoder.Host
{
    public class EncodingItemCreatedEventHandler : INotificationHandler<EncodingItemCreatedEvent>
    {
        private readonly IEventBus eventBus;
        public EncodingItemCreatedEventHandler(IEventBus eventBus)
        {
            this.eventBus = eventBus;
        }

        public Task Handle(EncodingItemCreatedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
