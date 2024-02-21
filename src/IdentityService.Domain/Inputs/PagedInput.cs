using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Domain
{
    public class PagedInput
    {
        public int SkipCount { get; set; }

        public int MaxResultCount { get; set; } = 10;
    }
}
