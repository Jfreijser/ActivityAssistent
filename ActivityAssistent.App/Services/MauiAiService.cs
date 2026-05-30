using System;
using System.Collections.Concurrent;
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

        private readonly ConcurrentDictionary<Guid, CancellationTokenSource> ActiveTasks = new();

        public async Task<MeetingAnalysisResultDto> AnalyzeMeetingAsync(AudioProcessingRequestDto RequestPayload, CancellationToken CancelToken)
        {
            var CancelTokenSource = CancellationTokenSource.CreateLinkedTokenSource(CancelToken);
            
            ActiveTasks.TryAdd(RequestPayload.AudioToken, CancelTokenSource);

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

        public async Task<bool> CancelAiAnalysisAsync(Guid Token)
        {
            throw new NotImplementedException();
        }

        public async Task<AiStatusDto> GetAiStatusAsync(Guid Token, CancellationToken CancelToken)
        {
            throw new NotImplementedException();
        }
    }
}
