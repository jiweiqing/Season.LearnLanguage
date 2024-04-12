using MediatR;

namespace MediaEncoder.Domain
{
    public class EncodingItemFailedEvent : INotification
    {
        public long Id { get; set; }
        public string SourceSystem { get; set; }
        public string ErrorMessage { get; set; }
        public EncodingItemFailedEvent(long id, string sourceSystem, string errorMessage)
        {
            Id = id;
            SourceSystem = sourceSystem;
            ErrorMessage = errorMessage;
        }
    }
}
