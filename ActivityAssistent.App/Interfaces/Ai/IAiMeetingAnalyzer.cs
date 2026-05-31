using System;
using System.Collections.Generic;
using System.Text;
using ActivityAssistent.Shared.Dtos.Ai;

namespace ActivityAssistent.App.Interfaces.Ai
{
    public  interface IAiMeetingAnalyzer
    {
        Task<MeetingAnalysisResultDto> AnalyzeMeetingAsync(AudioProcessingRequestDto RequestPayload, CancellationToken CancelToken);

        
        Task<AiStatusDto> GetAiStatusAsync(Guid Token, CancellationToken CancelToken);

        // 3. Noodrem: Stop een lopende analyse.
        Task<bool> CancelAiAnalysisAsync(Guid Token);

        Task<bool> SaveAnalysisResultsAsync(MeetingAnalysisResultDto Model, CancellationToken CancelToken);


    }
}

