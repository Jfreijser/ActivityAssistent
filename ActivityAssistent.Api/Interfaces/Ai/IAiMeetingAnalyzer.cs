using System;
using System.Collections.Generic;
using System.Text;
using ActivityAssistent.Shared.Dtos.Ai;

namespace ActivityAssistent.Api.Interfaces.Ai
{
    public  interface IAiMeetingAnalyzer
    {
        Task<MeetingAnalysisResultDto> AnalyzeMeetingAsync(AudioProcessingRequestDto RequestPayload, CancellationToken Token);
        
    }
}

