using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityAssistent.Shared.Dtos.Ai
{
    public class AudioProcessingRequestDto
    {
        public Guid AudioToken { get; set; }
        public Guid ConversationId { get; set; }
        public string filePath { get; set; }
    }
}
