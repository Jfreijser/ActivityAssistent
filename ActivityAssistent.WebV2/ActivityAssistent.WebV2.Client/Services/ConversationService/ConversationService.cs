using ActivityAssistent.Shared.Dtos.Conversations;
using ActivityAssistent.Shared.Interfaces.Conversations;

namespace ActivityAssistent.WebV2.Client.Services.ConversationService
{
    public class ConversationService(HttpClient Http) : IConversationService
    {
        public Task<ConversationDto> CreateConversationAsync(ConversationDto Conversation, CancellationToken Token)
        {
            throw new NotImplementedException();
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
