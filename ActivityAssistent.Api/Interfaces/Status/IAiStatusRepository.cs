using ActivityAssistent.Shared.Dtos.Ai;
using ActivityAssistent.Shared.Enums;

namespace ActivityAssistent.Api.Interfaces.Status
{
    public interface IAiStatusRepository : IStatusRepository<AiStatusDto, AiStatus>
    {
        Task <bool> SaveInitialStatusAsync(AudioProcessingRequestDto StatusRecord, CancellationToken CancellationToken);
        Task<bool> SaveAnalysisResultsAsync(MeetingAnalysisResultDto Model, CancellationToken CancelToken);

        Task<bool> UpdateAnalysisResultsAsync(MeetingAnalysisResultDto Model, CancellationToken CancelToken);
        Task<IReadOnlyList<AiAnalysisStateRecord>> GetAnalysisStateByConversationIdAsync(Guid ConversationId, CancellationToken CancelToken);
        Task<bool> DeleteAnalysisResultsAsync(Guid Token, CancellationToken CancelToken);
    }
}
