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
        //public async Task<string> TranscribeAudioAsync(byte[] AudioData, Guid AudioToken, CancellationToken CancelToken)
        //{
        //    await AiStatusRepository.UpdateStatusAsync(AudioToken, AiStatus.UploadingToSpeechService);

        //    string SpeechKey = "FJThRmTswDmj1aagpjBCDwzSmeDVtxdnFqzan8Sy8DbYQkDvZCKFJQQJ99CEACPV0roXJ3w3AAAYACOG0KyF";
        //    string SpeechRegion = "germanywestcentral";

        //    var SpeechConfiguration = SpeechConfig.FromSubscription(SpeechKey, SpeechRegion);

        //    string[] ExpectedLanguages = new string[] { "nl-NL", "en-US" };
        //    var LanguageDetectionConfiguration = AutoDetectSourceLanguageConfig.FromLanguages(ExpectedLanguages);

        //    using var PushStream = AudioInputStream.CreatePushStream();
        //    using var AudioConfiguration = AudioConfig.FromStreamInput(PushStream);

        //    // create the response 
        //    var FullTranscriptBuilder = new StringBuilder();


        //    var RecognitionTaskSource = new TaskCompletionSource<string>();

        //    using var Recognizer = new SpeechRecognizer(SpeechConfiguration, LanguageDetectionConfiguration, AudioConfiguration);


        //    Recognizer.Recognized += (Sender, EventArguments) =>
        //    {
        //        if (EventArguments.Result.Reason == ResultReason.RecognizedSpeech)
        //        {

        //            FullTranscriptBuilder.Append(EventArguments.Result.Text + " ");
        //        }
        //    };
        //    Recognizer.Recognizing += (Sender, EventArguments) =>
        //    {
        //        Console.WriteLine($"[DEBUG AZURE LUISTERT]: {EventArguments.Result.Text}");
        //    };


        //    Recognizer.Canceled += (Sender, EventArguments) =>
        //    {
        //        if (EventArguments.Reason == CancellationReason.Error)
        //        {
        //            RecognitionTaskSource.TrySetException(new Exception($"Azure Speech Error: {EventArguments.ErrorDetails}"));
        //        }
        //        else
        //        {
        //            RecognitionTaskSource.TrySetResult(FullTranscriptBuilder.ToString().Trim());
        //        }
        //    };

        //    // if the stream is done 
        //    Recognizer.SessionStopped += (Sender, EventArguments) =>
        //    {

        //        RecognitionTaskSource.TrySetResult(FullTranscriptBuilder.ToString().Trim());
        //    };

        //    await AiStatusRepository.UpdateStatusAsync(AudioToken, AiStatus.TranscribingAudio);
        //    // Koppel de CancellationToken uit de applicatie aan deze herkenning
        //    using var Registration = CancelToken.Register(() =>
        //    {
        //        // Als het token wordt geactiveerd, stoppen we de recognizer direct
        //        _ = Recognizer.StopContinuousRecognitionAsync();
        //        RecognitionTaskSource.TrySetCanceled(CancelToken);
        //    });
        //    await Recognizer.StartContinuousRecognitionAsync();

        //    // Duw de bytes in de stream. Azure begint nu op de achtergrond direct te verwerken
        //    PushStream.Write(AudioData);
        //    PushStream.Close(); // Sluit de stream zodat Azure weet: er komt niets meer aan

        //    // Wacht asynchroon totdat SessionStopped of Canceled de TaskSource vult
        //    string FinalResult = await RecognitionTaskSource.Task;

        //    // Stop de herkenning netjes voor het opruimen van resources
        //    await Recognizer.StopContinuousRecognitionAsync();


        //    if (string.IsNullOrWhiteSpace(FinalResult))
        //    {
        //        await AiStatusRepository.UpdateStatusAsync(AudioToken, AiStatus.ErrorTranscription);
        //        return "An error occurred while processing the audio.";

        //    }
        //    await AiStatusRepository.UpdateStatusAsync(AudioToken, AiStatus.TranscriptionCompleted);
        //    return FinalResult;

        //}

        public async Task<string> TranscribeAudioAsync(byte[] AudioData, Guid AudioToken, CancellationToken CancelToken)
        {
            await AiStatusRepository.UpdateStatusAsync(AudioToken, AiStatus.UploadingToSpeechService);

            // 1. Configuratie ophalen (Let op: Dit zijn nu je nieuwe AI Studio / OpenAI keys!)
            string AiStudioKey = Environment.GetEnvironmentVariable("AISTUDIO_KEY");
            string AiStudioEndpoint = Environment.GetEnvironmentVariable("AISTUDIO_ENDPOINT"); // Bijv: https://jouw-resource.openai.azure.com/

            // De naam die je zometeen in Azure AI Studio aan je Whisper-model geeft
            string DeploymentName = "whisper";
            string ApiVersion = "2024-02-01"; // De vaste API versie van Microsoft voor Whisper

            // 2. De specifieke URL opbouwen voor audioconversie
            string RequestUrl = $"{AiStudioEndpoint.TrimEnd('/')}/openai/deployments/{DeploymentName}/audio/transcriptions?api-version={ApiVersion}";

            // 3. Het digitale "formulier" opbouwen
            using var FormData = new MultipartFormDataContent();

            // Voeg de audio bytes toe. We doen alsof het een .m4a bestand is, Whisper lost de rest op.
            var AudioContent = new ByteArrayContent(AudioData);
            AudioContent.Headers.ContentType = MediaTypeHeaderValue.Parse("audio/m4a");
            FormData.Add(AudioContent, "file", "opname.m4a");

            // Optioneel: Stuur parameters mee (Nederlands afdwingen en direct platte tekst terugvragen)
            FormData.Add(new StringContent("nl"), "language");
            FormData.Add(new StringContent("text"), "response_format");

            // 4. Het verzoek inrichten
            using var RequestMessage = new HttpRequestMessage(HttpMethod.Post, RequestUrl);
            RequestMessage.Headers.Add("api-key", AiStudioKey);
            RequestMessage.Content = FormData;

            try
            {
                await AiStatusRepository.UpdateStatusAsync(AudioToken, AiStatus.TranscribingAudio);

                // 5. Verstuur het naar Azure (hier wacht de code totdat Whisper klaar is)
                var Response = await Http.SendAsync(RequestMessage, CancelToken);

                if (Response.IsSuccessStatusCode)
                {
                    // Omdat we response_format 'text' vroegen, is de content direct de uitgeschreven string
                    string FinalTranscript = await Response.Content.ReadAsStringAsync(CancelToken);

                    await AiStatusRepository.UpdateStatusAsync(AudioToken, AiStatus.TranscriptionCompleted);
                    return FinalTranscript.Trim();
                }
                else
                {
                    // Handig voor debugging als Azure de aanroep weigert
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
