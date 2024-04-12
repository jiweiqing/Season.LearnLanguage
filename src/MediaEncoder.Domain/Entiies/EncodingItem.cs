using Learning.Domain;
using Yitter.IdGenerator;

namespace MediaEncoder.Domain
{
    public class EncodingItem : CreationEntity, IAggregateRoot
    {
        public EncodingItem(long id) : base(id)
        {
        }

        /// <summary>
        /// 源系统
        /// </summary>
        public string SourceSystem { get; set; } = string.Empty;

        /// <summary>
        /// 文件大小 字节
        /// </summary>
        public long? FileByteSize { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// 两个文件的大小和散列值（SHA256）都相同的概率非常小。因此只要大小和SHA256相同，就认为是相同的文件。
        /// SHA256的碰撞的概率比MD5低很多。
        /// </summary>
        public string? FileHash { get; set; }

        /// <summary>
        /// 待转码的文件
        /// </summary>
        public string SourceUrl { get; set; } = string.Empty;

        /// <summary>
        /// 转码完成的路径
        /// </summary>
        public string? OutputUrl { get; set; }

        /// <summary>
        /// 转码目标类型，比如m4a、mp4等
        /// </summary>
        public string OutputFormat { get; set; } = string.Empty;

        /// <summary>
        /// 状态
        /// </summary>
        public ItemStatus Status { get;  set; }

        /// <summary>
        /// 转码工具的输出日志
        /// </summary>
        public string? LogText { get; private set; }

        public void Start()
        {
            this.Status = ItemStatus.Started;
            AddDomainEvent(new EncodingItemStartedEvent(Id,SourceSystem));
        }

        public void Complete(string outputUrl)
        {
            this.Status = ItemStatus.Completed;
            this.OutputUrl = outputUrl;
            this.LogText = "转码成功";
            AddDomainEvent(new EncodingItemCompletedEvent(Id, SourceSystem, outputUrl));
        }

        public void Fail(string logText)
        {
            //todo：通过集成事件写入Logging系统
            this.Status = ItemStatus.Failed;
            this.LogText = logText;
            AddDomainEventIfAbsent(new EncodingItemFailedEvent(Id, SourceSystem, logText));
        }

        public void Fail(Exception ex)
        {
            Fail($"转码处理失败：{ex}");
        }

        public void ChangeFileMeta(long fileSize, string hash)
        {
            this.FileByteSize = fileSize;
            this.FileHash = hash;
        }

        public static EncodingItem Create(long id, string name, string sourceUrl, string outputFormat, string sourceSystem)
        {
            // TODO: 需要加注入以及种子号
            EncodingItem item = new EncodingItem(YitIdHelper.NextId())
            {
                Id = id,
                CreationTime = DateTime.Now,
                FileName = name,
                OutputFormat = outputFormat,
                SourceUrl = sourceUrl,
                Status = ItemStatus.Ready,
                SourceSystem = sourceSystem,
            };
            item.AddDomainEvent(new EncodingItemCreatedEvent(item));
            return item;
        }
    }
}
