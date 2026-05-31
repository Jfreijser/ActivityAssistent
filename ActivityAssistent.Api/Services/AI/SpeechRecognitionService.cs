using System.Text;
using ActivityAssistent.Api.Infrastructure.Repositories.DapperRepository;
using ActivityAssistent.Api.Interfaces.Ai;
using ActivityAssistent.Api.Interfaces.Status;
using ActivityAssistent.Shared.Enums;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Crm.Sdk.Messages;
using System.Net.Http.Headers;

namespace ActivityAssistent.Api.Services.AI
{
    public class SpeechRecognitionService(IAiStatusRepository AiStatusRepository, HttpClient Http) : ISpeechRecognitionService
    {
       
        public async Task<string> TranscribeAudioAsync(byte[] AudioData, Guid AudioToken, CancellationToken CancelToken)
        {
            await AiStatusRepository.UpdateStatusAsync(AudioToken, AiStatus.UploadingToSpeechService);
            
            string AiStudioKey = Environment.GetEnvironmentVariable("AISTUDIO_KEY");
            string AiStudioEndpoint = Environment.GetEnvironmentVariable("AISTUDIO_ENDPOINT"); // Bijv: https://jouw-resource.openai.azure.com/

            // De naam van mijn whisper model
            string DeploymentName = "whisper";
            string ApiVersion = "2024-02-01"; // De vaste API versie van microsoft voor Whisper

            string RequestUrl = $"{AiStudioEndpoint.TrimEnd('/')}/openai/deployments/{DeploymentName}/audio/transcriptions?api-version={ApiVersion}";

            
            using var FormData = new MultipartFormDataContent();

            var AudioContent = new ByteArrayContent(AudioData);
            AudioContent.Headers.ContentType = MediaTypeHeaderValue.Parse("audio/m4a");
            FormData.Add(AudioContent, "file", "opname.m4a");

            
            FormData.Add(new StringContent("nl"), "language");
            FormData.Add(new StringContent("text"), "response_format");

            // 4. Het verzoek inrichten
            using var RequestMessage = new HttpRequestMessage(HttpMethod.Post, RequestUrl);
            RequestMessage.Headers.Add("api-key", AiStudioKey);
            RequestMessage.Content = FormData;

            try
            {
                await AiStatusRepository.UpdateStatusAsync(AudioToken, AiStatus.TranscribingAudio);

                var Response = await Http.SendAsync(RequestMessage, CancelToken);

                if (Response.IsSuccessStatusCode)
                {
                    string FinalTranscript = await Response.Content.ReadAsStringAsync(CancelToken);

                    await AiStatusRepository.UpdateStatusAsync(AudioToken, AiStatus.TranscriptionCompleted);
                    return FinalTranscript.Trim();
                }
                else
                {
                    
                    string ErrorContent = await Response.Content.ReadAsStringAsync(CancelToken);
                    await AiStatusRepository.UpdateStatusAsync(AudioToken, AiStatus.ErrorTranscription);
                    return $"API Fout ({Response.StatusCode}): {ErrorContent}";
                }
            }
            catch (OperationCanceledException)
            {
                await AiStatusRepository.UpdateStatusAsync(AudioToken, AiStatus.ErrorTranscription);
                return "Transcriberen is geannuleerd door de gebruiker (of door de 100-seconden timeout).";
            }
            catch (Exception Ex)
            {
                await AiStatusRepository.UpdateStatusAsync(AudioToken, AiStatus.ErrorTranscription);
                return $"Systeemfout: {Ex.Message}";
            }
        }
    }
}
