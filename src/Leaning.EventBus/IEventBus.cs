using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leaning.EventBus
{
    /// <summary>
    /// 事件总线
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventData"></param>
        void Publish(string eventName, object? eventData);
        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="handlerType"></param>
        void Subscribe(string eventName, Type handlerType);
        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="handlerType"></param>
        void Unsubscribe(string eventName,Type handlerType);
    }
}
