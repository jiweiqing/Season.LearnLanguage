namespace Listening.Admin.Host
{
    public class SentenceDto
    {
        public SentenceDto(double startTime, double endTime, string value) 
        {
            StartTime = startTime;
            EndTime = endTime;
            Value = value;
        }
        /// <summary>
        /// 开始时间
        /// </summary>
        public double StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public double EndTime { get; set; }
        /// <summary>
        /// 语句内容
        /// </summary>
        public string Value { get; set; } = string.Empty;
    }
}
