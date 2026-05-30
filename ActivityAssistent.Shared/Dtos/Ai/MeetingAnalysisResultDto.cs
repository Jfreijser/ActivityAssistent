using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityAssistent.Shared.Dtos.Ai
{
    public class MeetingAnalysisResultDto
    {
        public Guid AudioToken { get; set; }
        public string filePath { get; set; } = string.Empty;
        public string Transcription { get; set; } = string.Empty;

    }
}
