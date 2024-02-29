using Learning.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Domain
{
    public class Permission : AggregateRoot
    {
        public Permission(long id) : base(id)
        {
        }

        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public long? ParentId { get; set; }
        public int SortOrder { get; set; }
    }
}
