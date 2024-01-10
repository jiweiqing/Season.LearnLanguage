using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Domain
{
    public class BaseEntity<TKey>
    {
        public BaseEntity(TKey id)
        {
            Id = id;
        }

        [Key]
        public TKey Id { get; set; }
    }
}
