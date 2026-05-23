using System.Net.Http.Json;
using ActivityAssistent.Shared.Dtos.Companies;
using ActivityAssistent.Shared.Dtos.Conversations;
using ActivityAssistent.WebV2.Client.Interfaces.Conversations;

namespace ActivityAssistent.WebV2.Client.Services.ConversationService
{
    public class ConversationService(HttpClient Http) : IConversationService
    {
        public async Task<ConversationDto> CreateConversationAsync(CreateConversationDto Conversation, CancellationToken Token)
        {
            var Response = await Http.PostAsJsonAsync("api/Conversation/create", Conversation, Token);
            if (Response.IsSuccessStatusCode)
            {
                return await Response.Content.ReadFromJsonAsync<ConversationDto>(cancellationToken: Token);
            }
            else
            {
                throw new HttpRequestException($"Error creating conversation: {Response.ReasonPhrase}");
            }
        }

        public Task DeleteConversationAsync(Guid ConversationId, CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ConversationDto>> GetAllAsync(CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public Task<ConversationDto> GetConversationAsync(Guid ConversationId, CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ConversationDto>> GetRecentConversationsAsync(CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public Task<ConversationDto> UpdateConversationAsync(ConversationDto conversation, CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public Task UploadAudioAsync(Guid ConversationId, byte[] AudioData, CancellationToken Token)
        {
            throw new NotImplementedException();
        }
    }
}
