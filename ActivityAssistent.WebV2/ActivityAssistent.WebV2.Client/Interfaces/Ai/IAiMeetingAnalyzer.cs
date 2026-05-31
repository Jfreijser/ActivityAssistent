using System;
using System.Collections.Generic;
using System.Text;
using ActivityAssistent.Shared.Dtos.Ai;

namespace ActivityAssistent.WebV2.Client.Interfaces.Ai
{
    public  interface IAiMeetingAnalyzer
    {
        Task<StartMeetingAnalysistDto> AnalyzeMeetingAsync(AudioProcessingRequestDto RequestPayload, CancellationToken Token);
        
    }
}

