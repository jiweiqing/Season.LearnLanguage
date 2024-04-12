using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaEncoder.Domain
{
    /// <summary>
    /// 转码器接口
    /// </summary>
    public interface IMediaEncoder
    {
        /// <summary>
        /// 判断转码器是否能进行目标格式的转码
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        bool Accept(string format);
        /// <summary>
        /// 执行转码任务
        /// </summary>
        /// <param name="srcFile">源文件</param>
        /// <param name="destFile">目标文件</param>
        /// <param name="format">目标格式</param>
        /// <param name="args"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task EncodeAsync(FileInfo srcFile, FileInfo destFile, string format, string[]? args, CancellationToken cancellationToken);
    }
}
