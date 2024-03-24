using MediatR;

namespace Listening.Domain
{
    public class EpisodeUpdatedEvent(Episode episode) : INotification
    {
        public Episode Episode { get; } = episode;
    }
}
