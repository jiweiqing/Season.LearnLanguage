using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning.Domain
{
    public class ModificationEntity : CreationEntity, IAuditedObject
    {
        public ModificationEntity(long id) : base(id)
        {
        }

        public long? ModifierId { get; set; }
        public DateTime? ModificationTime { get; set; }
    }
}
