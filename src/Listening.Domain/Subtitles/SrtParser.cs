using SubtitlesParser.Classes.Parsers;
using System.Text;

namespace Listening.Domain
{
    /// <summary>
    /// .srt or .vtt
    /// </summary>
    public class SrtParser : ISubtitleParser
    {
        public bool Accept(SubtitleType subtitleType)
        {
            return subtitleType == SubtitleType.srt || subtitleType == SubtitleType.vtt;
        }

        public IEnumerable<Sentence> Parse(string subtitle)
        {
            SubParser srtParser = new SubParser();
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(subtitle)))
            {
                var items = srtParser.ParseStream(ms);
                return items.Select(s => new Sentence
                {
                    StartTime = TimeSpan.FromMicroseconds(s.StartTime),
                    EndTime = TimeSpan.FromMicroseconds(s.EndTime),
                    Value = string.Join(", ", s.Lines)
                });
            }
        }
    }
}
