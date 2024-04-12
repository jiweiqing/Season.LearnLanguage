using Leaning.EventBus;
using MediaEncoder.Domain;
using MediaEncoder.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MediaEncoder.Host
{
    [EventName("MediaEncoding.Created")]
    public class MediaEncodingCreatedHandler : DynamicIntegrationEventHandler
    {
        private readonly IEventBus _eventBus;
        private readonly MediaDbContext _dbContext;
        public MediaEncodingCreatedHandler(IEventBus eventBus, MediaDbContext dbContext)
        {
            _eventBus = eventBus;
            _dbContext = dbContext;
        }

        public override async Task HandleDynamic(string eventName, dynamic eventData)
        {
            long mediaId = long.Parse(eventData.MediaId);
            string mediaUrl = eventData.MediaUrl;
            string sourceSystem = eventData.SourceSystem;
            string fileName = eventData.FileName;
            string outputFormat = eventData.OutputFormat;
            bool exists = await _dbContext.EncodingItems.AnyAsync(e => e.SourceUrl == mediaUrl && e.OutputFormat == outputFormat);
            if (exists) 
            {
                return;
            }

            //把任务插入数据库，也可以看作是一种事件，不一定非要放到MQ中才叫事件
            //没有通过领域事件执行，因为如果一下子来很多任务，领域事件就会并发转码，而这种方式则会一个个的转码
            //直接用另一端传来的MediaId作为EncodingItem的主键
            var encodeItem = EncodingItem.Create(mediaId, fileName, mediaUrl, outputFormat, sourceSystem);
            _dbContext.Add(encodeItem);
            await _dbContext.SaveChangesAsync();

        }
    }
}
