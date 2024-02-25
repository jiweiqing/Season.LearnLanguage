using IdentityService.Domain;

namespace IdentityService.Host
{
    public class FullAuditeEntityDto : ModificationEntityDto, IFullAuditedObject
    {
        public bool IsDeleted { get; set; }
        public long? DeleterId { get; set; }
        public DateTime? DeletionTime { get; set; }
    }
}
