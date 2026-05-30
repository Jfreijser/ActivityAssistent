using ActivityAssistent.Api.Interfaces.Ai;
using ActivityAssistent.Api.Interfaces.Status;
using ActivityAssistent.Shared.Dtos.Ai;
using ActivityAssistent.Shared.Enums;

namespace ActivityAssistent.Api.Services.AI
{
    public class AIMeetingAnalyzerService(ISpeechRecognitionService SpeechRecognitionService, IAiStatusRepository AiStatusRepository) : IAiMeetingAnalyzerService
    {
        public Task<bool> CancelAiAnalysisAsync(Guid Token)
        {
            throw new NotImplementedException();
        }

        public Task<AiStatusDto> GetAiStatusAsync(Guid Token, CancellationToken CancelToken)
        {
            throw new NotImplementedException();
        }

        public async Task<MeetingAnalysisResultDto> StartAnalyzeMeetingAsync(AudioProcessingRequestDto RequestPayload, CancellationToken Token)
        {
            if (!File.Exists(RequestPayload.filePath))
            { 
                throw new FileNotFoundException("Audio file not found.");

            }
            try
            {

                // create record in database 
                var result =  await AiStatusRepository.SaveInitialStatusAsync(RequestPayload, Token);

                if (result == false)
                {
                    throw new Exception("Failed to save initial status to database.");
                }
                // get audio file

                byte[] wavBytes = File.ReadAllBytes(RequestPayload.filePath);
                if (wavBytes == null || wavBytes.Length == 0)
                {
                    throw new Exception("Failed to read audio file.");
                }
                // todo Status updaten
                await AiStatusRepository.UpdateStatusAsync(RequestPayload.AudioToken, AiStatus.PreparingAudio);
                
                var Transcription = await SpeechRecognitionService.TranscribeAudioAsync(wavBytes, RequestPayload.AudioToken, Token);
                return new MeetingAnalysisResultDto
                {
                    AudioToken = RequestPayload.AudioToken,
                    filePath = RequestPayload.filePath,
                    Transcription = Transcription
                };
            }
            catch (Exception ex)
            {

                Console.WriteLine($"An error occurred during meeting analysis: {ex}");
                throw;
            }
            


        }
    }
}
