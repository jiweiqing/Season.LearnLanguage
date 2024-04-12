using MediatR;

namespace MediaEncoder.Domain
{
    public class EncodingItemCompletedEvent: INotification
    {
        public long Id { get; set; }
        public string SourceSystem { get; set; }
        public string OutputUrl { get; set; }
        public EncodingItemCompletedEvent(long id, string sourceSystem,string outputUrl)
        {
            Id = id;
            SourceSystem = sourceSystem;
            OutputUrl = outputUrl;
        }
    }
}
