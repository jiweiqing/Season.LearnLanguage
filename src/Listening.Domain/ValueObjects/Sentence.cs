using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listening.Domain
{
    /// <summary>
    /// 语句数据
    /// </summary>
    public class Sentence
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public TimeSpan StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public TimeSpan EndTime { get; set; }
        /// <summary>
        /// 语句内容
        /// </summary>
        public string Value {  get; set; } = string.Empty;
    }
}
