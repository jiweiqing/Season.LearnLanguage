using Listening.Domain;
using System.ComponentModel.DataAnnotations;

namespace Listening.Admin.Host
{
    public class UpdateAlbumDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(FieldConstants.MaxNameLength)]
        [Required]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(FieldConstants.MaxDescriptionLength)]
        public string? Description { get; set; }
    }
}
