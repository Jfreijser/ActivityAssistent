using System.Text;
using System.Text.Json;
using ActivityAssistent.Api.Interfaces.Ai;
using ActivityAssistent.Api.Interfaces.Status;
using ActivityAssistent.Shared.Dtos.Ai;
using ActivityAssistent.Shared.Enums;

namespace ActivityAssistent.Api.Services.AI
{
    public class AiActionPointService(HttpClient Http, IAiStatusRepository AiStatusRepository) : IAiAssistantService
    {
        public async Task<MeetingAnalysisResultDto> ExtractActionPointsAsync(string TranscriptionText, CancellationToken CancelToken)
        {

            await AiStatusRepository.UpdateStatusAsync(Guid.Empty, AiStatus.SendingToAiAssistant, CancelToken);

            string AiStudioKey = Environment.GetEnvironmentVariable("AISTUDIO_KEY");
            string AiStudioEndpoint = Environment.GetEnvironmentVariable("AISTUDIO_ENDPOINT");

            string ApiVersion = "2024-12-01-preview"; // De stabiele standaard voor chat modellen
            string DeploymentName = "o4-mini"; // Zorg dat dit overeenkomt met AI Studio

            // 1. De juiste route voor tekst: /chat/completions
            string RequestUrl = $"{AiStudioEndpoint.TrimEnd('/')}/openai/deployments/{DeploymentName}/chat/completions?api-version={ApiVersion}";

            var OpenAiPayload = new
            {
                model = DeploymentName,
                response_format = new { type = "json_object" },
                messages = new[]
                {
                    new
                    {
                        role = "system",
                        // De prompt is hier iets strakker gemaakt voor een 100% voorspelbaar JSON-object
                        content = $"You are a sales assistant. Today is {DateTime.Now}. Analyze the transcript and return a JSON object containing a 'Summary' (string) and an 'ActionPoints' array. Each item in ActionPoints must have: 'Description' (string, actionable task), 'DueDate' (ISO-8601 string, derived from context), and 'IsCompleted' (boolean, default false)."
                    },
                    new
                    {
                        role = "user",
                        content = TranscriptionText
                    }
                },
                temperature = 1
            };

            string JsonContent = JsonSerializer.Serialize(OpenAiPayload);
            var RequestContent = new StringContent(JsonContent, Encoding.UTF8, "application/json");

            // 2. We gebruiken nu je opgebouwde RequestUrl in plaats van de hardcoded string
            using var RequestMessage = new HttpRequestMessage(HttpMethod.Post, RequestUrl);

            // 3. We gebruiken je variabele sleutel
            RequestMessage.Headers.Add("api-key", AiStudioKey);
            RequestMessage.Content = RequestContent;

            var Response = await Http.SendAsync(RequestMessage, CancelToken);

            if (Response.IsSuccessStatusCode)
            {
                string RawAzureResponse = await Response.Content.ReadAsStringAsync(CancelToken);

                // 4. Haal de daadwerkelijke JSON string uit de Azure envelop
                using JsonDocument AzureDoc = JsonDocument.Parse(RawAzureResponse);
                string CleanJson = AzureDoc.RootElement
                                            .GetProperty("choices")[0]
                                            .GetProperty("message")
                                            .GetProperty("content")
                                            .GetString();

                // CleanJson is nu exact de string die je kunt omzetten naar je C# objecten!
                var result = JsonSerializer.Deserialize<MeetingAnalysisResultDto>(CleanJson);
                return result;
            }
            else
            {
                string ErrorContent = await Response.Content.ReadAsStringAsync(CancelToken);
                Console.WriteLine($"Azure AI Extractie Fout ({Response.StatusCode}): {ErrorContent}");
                throw new Exception($"Azure AI Extractie Fout ({Response.StatusCode}): {ErrorContent}");
            }
        }
    }
    
}
