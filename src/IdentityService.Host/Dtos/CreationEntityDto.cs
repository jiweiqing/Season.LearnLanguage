using Learning.Domain;

namespace IdentityService.Host
{
    public class CreationEntityDto: ICreationAuditedObject
    {
        public long? CreatorId { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
