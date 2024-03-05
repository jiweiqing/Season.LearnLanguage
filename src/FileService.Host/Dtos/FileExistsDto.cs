namespace FileService.Host
{
    public class FileExistsDto
    {
        /// <summary>
        /// 是否存在
        /// </summary>
        public bool IsExists { get; set; }
        /// <summary>
        /// url
        /// </summary>
        public string? Url { get; set; }
    }
}
