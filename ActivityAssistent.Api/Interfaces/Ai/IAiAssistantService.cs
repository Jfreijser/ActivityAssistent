namespace ActivityAssistent.Api.Interfaces.Ai
{
    public interface IAiAssistantService
    {
        Task<string> ExtractActionPointsAsync(string TranscriptionText, CancellationToken CancelToken);
    }
}
