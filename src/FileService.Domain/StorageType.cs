using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService.Domain
{
    public enum StorageType
    {
        /// <summary>
        /// 远程服务器
        /// </summary>
        Publich,
        /// <summary>
        /// 备份服务器
        /// </summary>
        Backup
    }
}
