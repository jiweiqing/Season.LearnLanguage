using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService.Infrastructure
{
    public class LocalStorageOptions
    {
        /// <summary>
        /// 根目录
        /// </summary>
        public string WorkingDir { get; set; } = string.Empty;
    }
}
