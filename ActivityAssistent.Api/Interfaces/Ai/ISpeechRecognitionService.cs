namespace ActivityAssistent.Api.Interfaces.Ai
{
    public interface ISpeechRecognitionService
    {
        Task<string> TranscribeAudioAsync(byte[] AudioData, Guid AudioToken, CancellationToken CancelToken);
    }
}