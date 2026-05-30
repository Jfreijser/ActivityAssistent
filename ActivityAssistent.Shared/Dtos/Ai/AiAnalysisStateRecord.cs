using System;
using System.Collections.Generic;
using System.Text;
using ActivityAssistent.Shared.Enums;

namespace ActivityAssistent.Shared.Dtos.Ai
{
    public class AiAnalysisStateRecord
    {
        public Guid Token { get; set; }
        public Guid ConversationId { get; set; }
        public string FilePath { get; set; }

        public AiStatus State { get; set; }

        public string Transcription { get; set; }
        public string AiSummary { get; set; }
        public bool IsCompleted { get; set; }
        public bool HasError { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
