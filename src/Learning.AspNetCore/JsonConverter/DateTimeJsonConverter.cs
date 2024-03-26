//using System.Text.Json;
//using System.Text.Json.Serialization;

//namespace Learning.AspNetCore
//{
//    /// <summary>
//    /// 日期格式化转换器
//    /// </summary>
//    public class DateTimeJsonConverter : JsonConverter<DateTime>
//    {
//        private readonly string _dateFormat;
//        public DateTimeJsonConverter()
//        {
//            _dateFormat = "yyyy-MM-dd HH:mm:ss";
//        }
//        public DateTimeJsonConverter(string dateFormatString)
//        {
//            _dateFormat = dateFormatString;
//        }
//        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//        {
//            string? str = reader.GetString();
//            if (str == null)
//            {
//                return default(DateTime);
//            }
//            else
//            {
//                return DateTime.Parse(str);
//            }
//        }

//        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
//        {
//            // 用服务器所在的时区
//            writer.WriteStringValue(value.ToString(_dateFormat));
//        }
//    }
//}
