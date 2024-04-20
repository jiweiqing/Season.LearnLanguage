namespace Learning.Domain
{
    public class PagedInput
    {
        public int SkipCount { get; set; }

        // TODO: 控制最大条目数量
        public int MaxResultCount { get; set; } = 10;
    }
}
