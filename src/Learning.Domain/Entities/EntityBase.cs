using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Learning.Domain
{
    public class EntityBase<TKey>
    {
        public EntityBase(TKey id)
        {
            Id = id;
        }

        // 领域事件集合
        private List<INotification> domainEvents = new();

        [Key]
        public TKey Id { get; set; }

        /// <summary>
        /// 添加领域事件
        /// </summary>
        /// <param name="eventItem"></param>
        public void AddDomainEvent(INotification eventItem)
        {
            domainEvents.Add(eventItem);
        }

        /// <summary>
        /// 如果不存在则添加
        /// </summary>
        /// <param name="eventItem"></param>
        public void AddDomainEventIfAbsent(INotification eventItem)
        {
            if (!domainEvents.Contains(eventItem))
            {
                domainEvents.Add(eventItem);
            }
        }

        /// <summary>
        /// 清空领域事件
        /// </summary>
        public void ClearDomainEvents()
        {
            domainEvents.Clear();
        }

        /// <summary>
        /// 获取领域事件集合
        /// </summary>
        /// <returns></returns>
        public IEnumerable<INotification> GetDomainEvents()
        {
            return domainEvents;
        }
    }
}
