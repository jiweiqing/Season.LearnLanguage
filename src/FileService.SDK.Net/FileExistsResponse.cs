using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService.SDK.Net
{
    public class FileExistsResponse
    {
        public bool IsExists { get; set; }
        public Uri? Uri { get; set; }
    }
}
