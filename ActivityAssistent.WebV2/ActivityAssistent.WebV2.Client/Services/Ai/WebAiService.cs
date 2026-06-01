using ActivityAssistent.Shared.Dtos.Ai;
using ActivityAssistent.Shared.Dtos.Response;
using ActivityAssistent.WebV2.Client.Interfaces.Ai;
using Microsoft.AspNetCore.Components.Authorization;

namespace ActivityAssistent.WebV2.Client.Services.Ai
{
    public class WebAiService(HttpClient Http, AuthenticationStateProvider AuthStateProvider) : BaseService(Http, AuthStateProvider), IAiMeetingAnalyzer
    {
        public async Task<StartMeetingAnalysistDto> AnalyzeMeetingAsync(AudioProcessingRequestDto RequestPayload, CancellationToken Token)
        {
            var result = await PostAsync<ApiResponse<StartMeetingAnalysistDto>>("api/ai/analyzeMeeting", RequestPayload, Token);
            if (result is { IsSuccess: true })
            {
                return result.Data;
            }

            return new StartMeetingAnalysistDto();
        }

        public async Task<IReadOnlyList<AiAnalysisStateRecord>> GetAnalysisStateByConversationIdAsync(Guid ConversationId, CancellationToken CancelToken)
        {
            try
            {
                var result = await GetAsync<ApiResponse<IReadOnlyList<AiAnalysisStateRecord>>>($"api/ai/AnalysisState/{ConversationId}", CancelToken);
                if (result is { IsSuccess: true })
                {
                    return result.Data ?? Array.Empty<AiAnalysisStateRecord>();
                }

                return Array.Empty<AiAnalysisStateRecord>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting analysis state: {ex.Message}");
                return Array.Empty<AiAnalysisStateRecord>();
            }
        }
    }
}
