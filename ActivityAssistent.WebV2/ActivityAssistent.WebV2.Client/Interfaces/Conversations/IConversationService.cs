using System;
using System.Collections.Generic;
using System.Text;
using ActivityAssistent.Shared.Dtos.Conversations;

namespace ActivityAssistent.WebV2.Client.Interfaces.Conversations
{
    public interface IConversationService
    {
        Task<ConversationDto> CreateConversationAsync(CreateConversationDto Conversation, CancellationToken Token);
        Task<ConversationDto> UpdateConversationAsync(UpdateConversationDto conversation, CancellationToken Token);
        Task DeleteConversationAsync(Guid ConversationId, CancellationToken Token);
        Task<ConversationDto> GetConversationAsync(Guid ConversationId, CancellationToken Token);

        Task<List<ConversationDto>> GetRecentConversationsAsync(CancellationToken Token);
        
        Task UploadAudioAsync(Guid ConversationId, byte[] AudioData, CancellationToken Token);
        Task<List<ConversationDto>> GetAllAsync(CancellationToken Token);
    }

    
}
