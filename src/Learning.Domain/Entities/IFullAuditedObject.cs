using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning.Domain
{
    public interface IFullAuditedObject: IAuditedObject
    {
        public bool IsDeleted { get; set; }
        public long? DeleterId { get; set; }
        public DateTime? DeletionTime { get; set; }
    }
}
