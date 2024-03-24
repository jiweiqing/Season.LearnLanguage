using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listening.Domain
{
    public interface ISubtitleParser
    {
        /// <summary>
        /// 本解析器是否能够解析指定类型的字幕
        /// </summary>
        /// <param name="subtitleType">字幕类型</param>
        /// <returns></returns>
        bool Accept(SubtitleType subtitleType);

        /// <summary>
        /// 解析字幕
        /// </summary>
        /// <param name="subtitle">字幕内容</param>
        /// <returns></returns>
        IEnumerable<Sentence> Parse(string subtitle);
    }
}
