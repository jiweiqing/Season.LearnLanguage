using MediatR;

namespace Listening.Domain
{
    public class EpisodeCreatedEvent(Episode episode) : INotification
    {
        public Episode Episode { get; } = episode;
    }
}
