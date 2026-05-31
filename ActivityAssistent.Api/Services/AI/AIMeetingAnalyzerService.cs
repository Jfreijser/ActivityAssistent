using ActivityAssistent.Api.Infrastructure;
using ActivityAssistent.Api.Interfaces.ActionPoint;
using ActivityAssistent.Api.Interfaces.Ai;
using ActivityAssistent.Api.Interfaces.Conversations;
using ActivityAssistent.Api.Interfaces.Identity;
using ActivityAssistent.Api.Interfaces.Status;
using ActivityAssistent.Shared.Dtos.ActionPoints;
using ActivityAssistent.Shared.Dtos.Ai;
using ActivityAssistent.Shared.Enums;

namespace ActivityAssistent.Api.Services.AI
{
    public class AIMeetingAnalyzerService(ISpeechRecognitionService SpeechRecognitionService, IAiAssistantService AiActionPointService, IAiStatusRepository AiStatusRepository, IUserContext UserContext, IActionPointRepository ActionPointRepository) : IAiMeetingAnalyzerService
    {
        public Task<bool> CancelAiAnalysisAsync(Guid Token)
        {
            throw new NotImplementedException();
        }

        public async Task<AiStatusDto?> GetAiStatusAsync(Guid Token, CancellationToken CancelToken)
        {
            var result = await AiStatusRepository.GetStatusByTokenAsync(Token, CancelToken);
            return result;
        }

        public async Task<bool> SaveAnalysisResultsAsync(MeetingAnalysisResultDto Model, CancellationToken CancelToken)
        {
            var ActionResults = new List<CreateActionPointDto>();
            foreach (var actionPoint in Model.ActionPoints)
            {
                var createActionPointDto = new CreateActionPointDto()
                {
                    ConversationId = Model.ConversationId.Value,
                    Description = actionPoint.Description,
                    SalesUserId = UserContext.CurrentUserId,
                    DueDate = actionPoint.DueDate.Value,
                    IsCompleted = actionPoint.IsCompleted,
                    SubNrId = UserContext.SubNrId.Value
                };
                await ActionPointRepository.CreateAsync(createActionPointDto, CancelToken);

            }


            
            var result = await AiStatusRepository.UpdateAnalysisResultsAsync(Model, CancelToken);
            return result;


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
                if (string.IsNullOrWhiteSpace(Transcription))
                {
                    throw new Exception("Failed to transcribe audio.");
                }
                var AiResponse = await AiActionPointService.ExtractActionPointsAsync(Transcription, Token);
                if (AiResponse == null)
                {
                    throw new Exception("Failed to extract action points from transcription.");
                }
                return AiResponse;



            }
            catch (Exception ex)
            {

                Console.WriteLine($"An error occurred during meeting analysis: {ex}");
                throw;
            }
            


        }
    }
}
