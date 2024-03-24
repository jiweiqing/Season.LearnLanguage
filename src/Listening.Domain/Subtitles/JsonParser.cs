using System.Text.Json;

namespace Listening.Domain
{
    public class JsonParser : ISubtitleParser
    {
        public bool Accept(SubtitleType subtitleType)
        {
            return subtitleType == SubtitleType.json;
        }

        public IEnumerable<Sentence> Parse(string subtitle)
        {
            return JsonSerializer.Deserialize<IEnumerable<Sentence>>(subtitle)!;
        }
    }
}
