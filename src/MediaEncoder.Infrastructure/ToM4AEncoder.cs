using FFmpeg.NET;
using Learning.Domain;
using MediaEncoder.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaEncoder.Infrastructure
{
    public class ToM4AEncoder : IMediaEncoder
    {
        public bool Accept(string format)
        {
            return "m4a".Equals(format, StringComparison.OrdinalIgnoreCase);
        }

        public async Task EncodeAsync(FileInfo srcFile, FileInfo destFile, string format, string[]? args, CancellationToken cancellationToken)
        {
            //可以用“FFmpeg.AutoGen”，因为他是bingding库，不用启动独立的进程，更靠谱。但是编程难度大，这里重点不是FFMPEG，所以先用命令行实现
            var inputFile = new InputFile(srcFile);
            var outputFile = new OutputFile(destFile);
            var baseDir = AppContext.BaseDirectory;
            string ffmpegPath = Path.Combine(baseDir, "ffmpeg.exe");
            var ffmpeg = new Engine(ffmpegPath);
            string? errorMsg = null;

            ffmpeg.Error += (s, e) =>
            {
                errorMsg = e.Exception.Message;
            };

            // 开始转码
            await ffmpeg.ConvertAsync(inputFile, outputFile, cancellationToken);

            if (errorMsg != null)
            {
                throw new Exception(errorMsg);
            }
        }
    }
}
