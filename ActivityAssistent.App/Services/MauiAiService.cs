using System;
using System.Collections.Generic;
using System.Text;
using ActivityAssistent.App.Auth;
using ActivityAssistent.App.Interfaces.Ai;
using ActivityAssistent.Shared.Dtos.Ai;
using ActivityAssistent.Shared.Dtos.Response;

namespace ActivityAssistent.App.Services
{
    public class MauiAiService(HttpClient Http, CustomAuthenticationStateProvider authStateProvider) : BaseService(Http, authStateProvider), IAiMeetingAnalyzer
    {

        public async Task<MeetingAnalysisResultDto> AnalyzeMeetingAsync(AudioProcessingRequestDto RequestPayload, CancellationToken CancelToken)
        {
            try
            {
                var result = await PostAsync<ApiResponse<MeetingAnalysisResultDto>>("api/ai/analyzeMeeting", RequestPayload, CancelToken);
                if (result.IsSuccess)
                {
                    return result.Data;
                }
                else
                {
                    Console.WriteLine($"Error analyzing meeting: {result.ErrorMessage}");
                    throw new Exception(result.ErrorMessage);
                }
            }
            catch (Exception ex )
            {
                Console.WriteLine($"Somthing Went wrong: {ex.Message}");
                throw;
            }
        }

        public async Task<AiStatusDto> GetAiStatusAsync(Guid Token, CancellationToken CancelToken)
        {
            try
            {
                var result = await GetAsync<ApiResponse<AiStatusDto>>($"api/ai/AiStatus/{Token}", CancelToken);
                if (result is { IsSuccess: true })
                {
                    return result.Data;
                }

                if (result == null)
                {
                    Console.WriteLine("Error getting AI status: empty response.");
                    return null;
                }

                Console.WriteLine($"Error getting AI status: {result.ErrorMessage}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting AI status: {ex.Message}");
                return null;
            }
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

        public async Task<bool> SaveAnalysisResultsAsync(MeetingAnalysisResultDto Model, CancellationToken CancelToken)
        {
            try
            {
                var result = await PostAsync<ApiResponse<bool>>("api/ai/SaveAnalysisActionPoints", Model, CancelToken);
                if (result.IsSuccess)
                {
                    return result.Data;
                }
                else
                {
                    Console.WriteLine($"Error saving analysis results: {result.ErrorMessage}");
                    return result.Data;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }
    }
}
