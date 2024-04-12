using MediaEncoder.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaEncoder.Infrastructure
{
    public class MediaEncoderRepository : IMediaEncoderRepository
    {
        private readonly MediaDbContext _mediaDbContext;
        public MediaEncoderRepository(MediaDbContext mediaDbContext) 
        {
            _mediaDbContext = mediaDbContext;
        }
        public async Task<EncodingItem?> FindCompelteItemAsync(string fileHash, long fileSize)
        {
            return await _mediaDbContext.EncodingItems.FirstOrDefaultAsync(e => e.FileHash == fileHash
                     && e.FileByteSize == fileSize && e.Status == ItemStatus.Completed);
        }

        public async Task<List<EncodingItem>> GetListAsync(ItemStatus itemStatus)
        {
            return await _mediaDbContext.EncodingItems.Where(e => e.Status == ItemStatus.Ready)
                .ToListAsync();
        }
    }
}
