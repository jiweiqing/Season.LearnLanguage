namespace Learning.Domain
{
    public class PagedInput
    {
        public int SkipCount { get; set; }

        public int MaxResultCount { get; set; } = 10;
    }
}
