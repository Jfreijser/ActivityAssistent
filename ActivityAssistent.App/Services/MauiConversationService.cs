using ActivityAssistent.App.Auth;
using ActivityAssistent.App.Interfaces.Conversations;
using ActivityAssistent.Shared.Dtos.Conversations;
using ActivityAssistent.Shared.Dtos.Response;

namespace ActivityAssistent.App.Services
{
    public class MauiConversationService(HttpClient Http, CustomAuthenticationStateProvider authStateProvider) : BaseService(Http, authStateProvider), IConversationService
    {
        public Task<ConversationDto> CreateConversationAsync(CreateConversationDto Conversation, CancellationToken Token)
        {
            return CreateConversationInternalAsync(Conversation, Token);
        }

        public Task DeleteConversationAsync(Guid ConversationId, CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public Task<List<ConversationDto>> GetAllAsync(CancellationToken Token)
        {
            return GetAllInternalAsync(Token);
        }

        public Task<ConversationDto> GetConversationAsync(Guid ConversationId, CancellationToken Token)
        {
            return GetConversationInternalAsync(ConversationId, Token);
        }

        public Task<List<ConversationDto>> GetRecentConversationsAsync(CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public Task<ConversationDto> UpdateConversationAsync(UpdateConversationDto conversation, CancellationToken Token)
        {
            return UpdateConversationInternalAsync(conversation, Token);
        }

        public Task UploadAudioAsync(Guid ConversationId, byte[] AudioData, CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        private async Task<ConversationDto> CreateConversationInternalAsync(CreateConversationDto Conversation, CancellationToken Token)
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

        private async Task<List<ConversationDto>> GetAllInternalAsync(CancellationToken Token)
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

                return respone.Data;
            }
            catch (Exception)
            {
                throw new Exception("Failed to fetch conversations.");

            }
        }

        private async Task<ConversationDto> GetConversationInternalAsync(Guid ConversationId, CancellationToken Token)
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

        private async Task<ConversationDto> UpdateConversationInternalAsync(UpdateConversationDto conversation, CancellationToken Token)
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
    }
}
