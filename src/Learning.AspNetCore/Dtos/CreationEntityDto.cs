using Learning.Domain;

namespace Learning.AspNetCore
{
    public class CreationEntityDto : EntityDtoBase, ICreationAuditedObject
    {
        public long? CreatorId { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
