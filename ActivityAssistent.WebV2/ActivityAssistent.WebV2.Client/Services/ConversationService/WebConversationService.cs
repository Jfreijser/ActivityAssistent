using System.Net.Http.Json;
using ActivityAssistent.Shared.Dtos.Companies;
using ActivityAssistent.Shared.Dtos.Conversations;
using ActivityAssistent.Shared.Dtos.Response;
using ActivityAssistent.WebV2.Client.Interfaces.Conversations;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.VisualBasic;

namespace ActivityAssistent.WebV2.Client.Services.ConversationService
{
    public class WebConversationService(HttpClient Http, AuthenticationStateProvider AuthStateProvider) : BaseService(Http, AuthStateProvider),IConversationService
    {
        public async Task<ConversationDto> CreateConversationAsync(CreateConversationDto Conversation, CancellationToken Token)
        {
            try
            {
                var Response = await PostAsync<ApiResponse<ConversationDto>>("api/Conversation/create", Conversation, Token);

                if (Response.IsSuccess)
                {
                    return Response.Data;
                }
                else
                {
                    return new ConversationDto();
                }
            }
            catch (Exception)
            {

                return new ConversationDto();
            }

           
        }

        public Task DeleteConversationAsync(Guid ConversationId, CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ConversationDto>> GetAllAsync(CancellationToken Token)
        {
            try
            {
                var respone = await GetAsync<ApiResponse<List<ConversationDto>>>("api/Conversation/getallAsync", Token);

                if (respone.Data.Count == 0)
                {
                    throw new HttpRequestException("The API response was empty or invalid.");
                }

                if (!respone.IsSuccess)
                {
                    throw new HttpRequestException($"Error fetching conversations: {respone.ErrorMessage}");
                    
                }

                // 3. Nu weet de compiler 100% zeker dat de data er is.
                // respone.Data is al een List<ConversationDto>, dus .ToList() is zelfs overbodig!
                return respone.Data;
            }
            catch (Exception)
            {
                throw new Exception("Failed to fetch conversations.");
               
            }
           

        }

        public async  Task<ConversationDto> GetConversationAsync(Guid ConversationId, CancellationToken Token)
        {
            try
            {
                var Response = await GetAsync<ApiResponse<ConversationDto>>($"api/Conversation/GetConversationById/{ConversationId}", Token);

                if (Response.IsSuccess)
                {
                    return Response.Data;
                }
                else
                {
                    return new ConversationDto();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[API FOUT] Updaten mislukt: {ex.Message}");
                return new ConversationDto();
            }

        }

        public Task<List<ConversationDto>> GetRecentConversationsAsync(CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        
        public async Task<ConversationDto> UpdateConversationAsync(UpdateConversationDto conversation, CancellationToken Token)
        {
            try
            {
                var result = await PutAsync<ApiResponse<ConversationDto>>("api/Conversation/update", conversation, Token);

                if (!result.IsSuccess)
                {
                    throw new HttpRequestException($"Error updating conversation: {result.ErrorMessage}");
                }

                if (result.Data == null)
                {
                    throw new HttpRequestException("The API update was successful, but the returned data was empty.");
                }

                return result.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[API FOUT] Updaten mislukt: {ex.Message}");
                throw;
            }
        }

        public Task UploadAudioAsync(Guid ConversationId, byte[] AudioData, CancellationToken Token)
        {
            throw new NotImplementedException();
        }
    }
}
