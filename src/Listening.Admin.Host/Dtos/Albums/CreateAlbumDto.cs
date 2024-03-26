using Listening.Domain;
using System.ComponentModel.DataAnnotations;

namespace Listening.Admin.Host
{
    public class CreateAlbumDto: UpdateAlbumDto
    {
        /// <summary>
        /// 分类id
        /// </summary>
        [Required]
        public long CategoryId { get; set; }
    }
}
