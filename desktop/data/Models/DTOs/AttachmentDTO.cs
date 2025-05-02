using System.Collections.Generic;

namespace desktop.data.Models.DTOs
{
    public class AttachmentDTO
    {
        public int Id { get; set; }
        public int AttachmentId { get; set; }
        public List<AttachmentInfoDTO>? AttachmentInfo { get; set; }
    }
}
