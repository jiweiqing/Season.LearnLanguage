using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaEncoder.Domain
{
    /// <summary>
    /// 开始转码事件
    /// </summary>
    public class EncodingItemStartedEvent : INotification
    {
        public long Id { get; set; }
        public string SourceSystem { get; set; }
        public EncodingItemStartedEvent(long id, string sourceSystem)
        {
            Id = id;
            SourceSystem = sourceSystem;
        }
    }
}
