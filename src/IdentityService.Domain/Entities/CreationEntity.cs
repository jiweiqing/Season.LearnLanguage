namespace IdentityService.Domain
{
    public class CreationEntity:BaseEntity<long>, ICreationAuditedObject
    {
        public CreationEntity(long id) : base(id)
        {
        }

        public long? CreatorId { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
