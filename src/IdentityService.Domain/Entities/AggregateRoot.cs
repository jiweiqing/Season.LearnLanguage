using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Domain
{
    public class AggregateRoot : DeletionEntity, IAggregateRoot
    {
        public AggregateRoot(long id) : base(id)
        {
        }
    }
}
