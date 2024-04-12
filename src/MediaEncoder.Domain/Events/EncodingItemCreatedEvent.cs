using MediatR;

namespace MediaEncoder.Domain
{
    public class EncodingItemCreatedEvent : INotification
    {
        public EncodingItem EncodingItem { get; set; }
        public EncodingItemCreatedEvent(EncodingItem encodingItem)
        {
            EncodingItem = encodingItem;
        }
    }
}
