namespace Learning.Domain
{
    public class CreationEntity:EntityBase<long>, ICreationAuditedObject
    {
        public CreationEntity(long id) : base(id)
        {
        }

        public long? CreatorId { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
