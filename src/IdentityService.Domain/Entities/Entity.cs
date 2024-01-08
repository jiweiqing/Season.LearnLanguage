using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Domain
{
    public class Entity<TKey>
    {
        public Entity(TKey id)
        {
            Id = id;
        }

        public TKey Id { get; set; }
    }
}
