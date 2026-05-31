using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityAssistent.Shared.Dtos.Ai
{
    public class MeetingAnalysisResultDto
    {
        public Guid Token { get; set; }
        public Guid? ConversationId { get; set; }
        public string Summary { get; set; }
        public List<AiExtractedActionPointDto> ActionPoints { get; set; }
    }
}
