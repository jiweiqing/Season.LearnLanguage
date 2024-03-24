using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listening.Domain
{
    /// <summary>
    /// 字幕解析工厂
    /// </summary>
    public class SubtitleParserFactory
    {
        private static List<ISubtitleParser> parsers = new();

        static SubtitleParserFactory()
        {
            // 扫描程序集中所有实现了ISubtitleParser接口的类
            var parserTypes = typeof(SubtitleParserFactory).Assembly.GetTypes()
                .Where(t => typeof(ISubtitleParser).IsAssignableFrom(t) && !t.IsAbstract);

            foreach (var parserType in parserTypes)
            {
                var subtitleParser = (ISubtitleParser?)Activator.CreateInstance(parserType);
                if (subtitleParser != null)
                {
                    parsers.Add(subtitleParser);
                }
            }
        }

        /// <summary>
        /// 将符合的解析器返回
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static ISubtitleParser? GetParser(SubtitleType typeName)
        {
            foreach (var parser in parsers)
            {
                if (parser.Accept(typeName))
                {
                    return parser;
                }
            }
            return null;
        }
    }
}
