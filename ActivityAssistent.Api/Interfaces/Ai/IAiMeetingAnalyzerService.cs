using System;
using System.Collections.Generic;
using System.Text;
using ActivityAssistent.Shared.Dtos.Ai;

namespace ActivityAssistent.Api.Interfaces.Ai
{
    public  interface IAiMeetingAnalyzerService
    {
        Task<MeetingAnalysisResultDto> StartAnalyzeMeetingAsync(AudioProcessingRequestDto RequestPayload, CancellationToken Token);
        Task<AiStatusDto?> GetAiStatusAsync(Guid Token, CancellationToken CancelToken);
        Task<IReadOnlyList<AiAnalysisStateRecord>> GetAnalysisStateByConversationIdAsync(Guid ConversationId, CancellationToken CancelToken);
        Task<bool> CancelAiAnalysisAsync(Guid Token);
        Task<bool> SaveAnalysisResultsAsync(MeetingAnalysisResultDto Model, CancellationToken CancelToken);

    }
}

