namespace IdentityService.Host.Dtos
{
    public class PagedResult<T>
    {
        public PagedResult()
        {
            Items = new List<T>();
        }
        public PagedResult(List<T> items, long totalCount)
        {
            Items = items;
            TotalCount = totalCount;
        }
        public long TotalCount { get; set; }

        public long PageNumber { get; private set; }

        public long PageSize { get; private set; }

        public long TotalPages { get; private set; }
        public List<T> Items { get; set; }
    }
}
