//using Leaning.EventBus;
//using Listening.Admin.Host.Hubs;
//using Listening.Domain;
//using Listening.Infrastructure;
//using Microsoft.AspNetCore.SignalR;
//using System.Xml.Linq;

//namespace Listening.Admin.Host.EventHandlers
//{

//    //收听转码服务发出的集成事件
//    //把状态通过SignalR推送给客户端，从而显示“转码进度”
//    [EventName("MediaEncoding.Started")]
//    [EventName("MediaEncoding.Failed")]
//    [EventName("MediaEncoding.Duplicated")]
//    [EventName("MediaEncoding.Completed")]
//    class MediaEncodingStatusChangeIntegrationHandler : DynamicIntegrationEventHandler
//    {
//        private readonly ListeningDbContext dbContext;
//        private readonly IEpisodeRepository _repository;
//        private readonly EncodingEpisodeHelper encHelper;
//        private readonly IHubContext<EpisodeEncodingStatusHub> hubContext;

//        public MediaEncodingStatusChangeIntegrationHandler(ListeningDbContext dbContext,
//            EncodingEpisodeHelper encHelper,
//            IHubContext<EpisodeEncodingStatusHub> hubContext, IEpisodeRepository repository)
//        {
//            this.dbContext = dbContext;
//            this.encHelper = encHelper;
//            this.hubContext = hubContext;
//            this._repository = repository;
//        }

//        public override async Task HandleDynamic(string eventName, dynamic eventData)
//        {
//            string sourceSystem = eventData.SourceSystem;
//            if (sourceSystem != "Listening")//可能是别的系统的转码消息
//            {
//                return;
//            }
//            long id = long.Parse(eventData.Id);//EncodingItem的Id就是Episode 的Id

//            switch (eventName)
//            {
//                case "MediaEncoding.Started":
//                    await encHelper.UpdateEpisodeStatusAsync(id, "Started");
//                    await hubContext.Clients.All.SendAsync("OnMediaEncodingStarted", id);//通知前端刷新
//                    break;
//                case "MediaEncoding.Failed":
//                    await encHelper.UpdateEpisodeStatusAsync(id, "Failed");
//                    //todo: 这样做有问题，这样就会把消息发送给所有打开这个界面的人，应该用connectionId、userId等进行过滤，
//                    await hubContext.Clients.All.SendAsync("OnMediaEncodingFailed", id);
//                    break;
//                case "MediaEncoding.Duplicated":
//                    await encHelper.UpdateEpisodeStatusAsync(id, "Completed");
//                    await hubContext.Clients.All.SendAsync("OnMediaEncodingCompleted", id);//通知前端刷新
//                    break;
//                case "MediaEncoding.Completed":
//                    //转码完成，则从Redis中把暂存的Episode信息取出来，然后正式地插入Episode表中
//                    await encHelper.UpdateEpisodeStatusAsync(id, "Completed");
//                    Uri outputUrl = new Uri(eventData.OutputUrl);
//                    var encItem = await encHelper.GetEncodingEpisodeAsync(id);

//                    long albumId = encItem.AlbumId;
//                    int maxOrder = await _repository.GetMaxSortOrderAsync(albumId);
//                    /*
//                    Episode episode = Episode.Create(id, maxSeq.Value + 1, encodingEpisode.Name, albumId, outputUrl,
//                        encodingEpisode.DurationInSecond, encodingEpisode.SubtitleType, encodingEpisode.Subtitle);*/
//                    var episopde = Episode.Create(maxOrder, encItem.Name, albumId, encItem.Resource, encItem.Duration, encItem.Subtitle, encItem.SubtitleType);

//                    dbContext.Add(episopde);
//                    await dbContext.SaveChangesAsync();
//                    await hubContext.Clients.All.SendAsync("OnMediaEncodingCompleted", id);//通知前端刷新
//                    break;
//                default:
//                    throw new ArgumentOutOfRangeException(nameof(eventName));
//            }
//        }
//    }
//}
