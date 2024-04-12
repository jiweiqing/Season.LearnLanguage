using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaEncoder.Domain
{
    public interface IMediaEncoderRepository
    {
        /// <summary>
        /// 查找转码完成的记录
        /// </summary>
        /// <param name="fileHash"></param>
        /// <param name="fileSize"></param>
        /// <returns></returns>
        Task<EncodingItem?> FindCompelteItemAsync(string fileHash, long fileSize);
        /// <summary>
        /// 依据状态查找记录列表
        /// </summary>
        /// <param name="itemStatus"></param>
        /// <returns></returns>
        Task<List<EncodingItem>> GetListAsync(ItemStatus itemStatus);
    }
}
