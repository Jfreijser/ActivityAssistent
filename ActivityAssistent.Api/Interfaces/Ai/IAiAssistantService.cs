using ActivityAssistent.Shared.Dtos.Ai;

namespace ActivityAssistent.Api.Interfaces.Ai
{
    public interface IAiAssistantService
    {
        Task<MeetingAnalysisResultDto> ExtractActionPointsAsync(string TranscriptionText, CancellationToken CancelToken);
    }
}
