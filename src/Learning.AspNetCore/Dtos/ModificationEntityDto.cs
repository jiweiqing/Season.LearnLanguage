using Learning.Domain;

namespace Learning.AspNetCore
{
    public class ModificationEntityDto: CreationEntityDto, IAuditedObject
    {
        public long? ModifierId { get; set; }
        public DateTime? ModificationTime { get; set; }
    }
}
