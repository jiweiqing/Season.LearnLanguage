using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning.Domain
{
    public interface IAuditedObject: ICreationAuditedObject
    {
        /// <summary>
        /// 修改人ID
        /// </summary>
        public long? ModifierId { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModificationTime { get; set; }
    }
}
