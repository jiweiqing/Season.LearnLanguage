using Learning.Domain;

namespace Learning.AspNetCore
{
    public class FullAuditeEntityDto : ModificationEntityDto, IFullAuditedObject
    {
        public bool IsDeleted { get; set; }
        public long? DeleterId { get; set; }
        public DateTime? DeletionTime { get; set; }
    }
}
