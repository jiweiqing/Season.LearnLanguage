using Learning.Domain;
using System.Data;
using System.Xml.Linq;
using Yitter.IdGenerator;

namespace Listening.Domain
{
    public class Episode : AggregateRoot
    {
        public Episode(long id) : base(id)
        {
        }

        /// <summary>
        /// 专辑id
        /// </summary>
        public long AlbumId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// 音频资源url
        /// </summary>
        public string Resource { get; set; } = string.Empty;
        /// <summary>
        /// 时长,单位秒
        /// </summary>
        public double Duration { get; set; }
        /// <summary>
        /// 字幕
        /// </summary>
        public string Subtitle { get; set; } = string.Empty;
        /// <summary>
        /// 字幕类型
        /// </summary>
        public SubtitleType SubtitleType { get; set; }
        /// <summary>
        /// 是否可用
        /// </summary>
        public bool IsEabled { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int SortOrder {  get; set; }

        public Episode ChangeSortOrder(int sortOrder)
        {
            SortOrder = sortOrder;
            // 发送事件
            AddDomainEventIfAbsent(new EpisodeUpdatedEvent(this));
            return this;
        }

        public Episode ChangeName(string name)
        {
            Name = name;
            AddDomainEventIfAbsent(new EpisodeUpdatedEvent(this));
            return this;
        }

        public Episode ChangeSubtitle(SubtitleType subtitleType, string subtitle)
        {
            var parser = SubtitleParserFactory.GetParser(subtitleType);
            if (parser == null)
            {
                throw new ArgumentOutOfRangeException(nameof(subtitleType), $"subtitleType={subtitleType} is not supported.");
            }

            SubtitleType = subtitleType;
            Subtitle = subtitle;
            AddDomainEventIfAbsent(new EpisodeUpdatedEvent(this));
            return this;
        }

        public static Episode Create(
            int sortOrder, string name, long albumId,
            string resource, double duration, string subtitle,
            SubtitleType subtitleType)
        {
            var parser = SubtitleParserFactory.GetParser(subtitleType);
            if (parser == null)
            {
                throw new ArgumentOutOfRangeException(nameof(subtitleType), $"subtitleType={subtitleType} is not supported.");
            }

            //新建的时候默认可见
            Episode episode = new Episode(YitIdHelper.NextId())
            {
                SortOrder = sortOrder,
                Name = name,
                AlbumId = albumId,
                Resource = resource,
                Duration = duration,
                Subtitle = subtitle,
                SubtitleType = subtitleType,
                IsEabled = false
            };

            episode.AddDomainEvent(new EpisodeCreatedEvent(episode));

            return episode;
        }


        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="albumId"></param>
        /// <param name="name"></param>
        /// <param name="resource"></param>
        /// <param name="duration"></param>
        /// <param name="subtitle"></param>
        /// <param name="subtitleType"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        public Episode Update(
            string name, 
            string resource, double duration, 
            string subtitle, SubtitleType subtitleType)
        {
            Name = name;
            Resource = resource;
            Duration = duration;
            Subtitle = subtitle;
            SubtitleType = subtitleType;
            return this;
        }

        public Episode Hide()
        {
            IsEabled = false;
            AddDomainEventIfAbsent(new EpisodeUpdatedEvent(this));
            return this;
        }
        public Episode Show()
        {
            IsEabled = true;
            AddDomainEventIfAbsent(new EpisodeUpdatedEvent(this));
            return this;
        }

        public void Delete()
        {
            AddDomainEventIfAbsent(new EpisodeUpdatedEvent(this));
        }

        public IEnumerable<Sentence> ParseSubtitle()
        {
            var parser = SubtitleParserFactory.GetParser(this.SubtitleType);
            if (parser!=null)
            {
                return parser.Parse(this.Subtitle);
            }
            return new List<Sentence>();
        }
    }
}
