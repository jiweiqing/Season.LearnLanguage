using IdentityService.Domain;

namespace IdentityService.Host
{
    public class ModificationEntityDto: CreationEntityDto, IAuditedObject
    {
        public long? ModifierId { get; set; }
        public DateTime? ModificationTime { get; set; }
    }
}
