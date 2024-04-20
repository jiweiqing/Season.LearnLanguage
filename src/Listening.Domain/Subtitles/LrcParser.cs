using Learning.Domain;
using Opportunity.LrcParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listening.Domain
{
    public class LrcParser : ISubtitleParser
    {
        public bool Accept(SubtitleType subtitleType)
        {
            return subtitleType == SubtitleType.lrc;
        }

        public IEnumerable<Sentence> Parse(string subtitle)
        {
            var lyrics = Lyrics.Parse(subtitle);
            if (lyrics.Exceptions.Count>0)
            {
                throw new BusinessException("lrc解析失败");
            }
            lyrics.Lyrics.PreApplyOffset();//应用上[offset:500]这样的偏移

            return FromLrc(lyrics.Lyrics);
        }

        private static Sentence[] FromLrc(Lyrics<Line> lyrics)
        {
            var lines = lyrics.Lines;
            Sentence[] sentences = new Sentence[lines.Count];
            for (int i = 0; i < lines.Count - 1; i++)
            {
                var line = lines[i];
                var nextLine = lines[i + 1];
                Sentence sentence = new Sentence()
                {
                    StartTime = line.Timestamp.TimeOfDay,
                    EndTime = nextLine.Timestamp.TimeOfDay,
                    Value = line.Content
                };
                sentences[i] = sentence;
            }

            // lastLine
            var lastLine = lines.Last();
            TimeSpan lastLineStartTime = lastLine.Timestamp.TimeOfDay;

            //lrc没有结束时间，就极端假定最后一句耗时1分钟
            TimeSpan lastLineEndTime = lastLineStartTime.Add(TimeSpan.FromMinutes(1));

            Sentence lastSentence = new Sentence()
            {
                StartTime = lastLineStartTime,
                EndTime = lastLineEndTime,
                Value = lastLine.Content
            };

            sentences[sentences.Count() - 1] = lastSentence;

            return sentences;
        }
    }
}
