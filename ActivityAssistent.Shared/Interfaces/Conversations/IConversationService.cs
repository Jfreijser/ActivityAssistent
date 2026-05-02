using System;
using System.Collections.Generic;
using System.Text;
using ActivityAssistent.Shared.Dtos.Conversations;

namespace ActivityAssistent.Shared.Interfaces.Conversations
{
    public interface IConversationService
    {
        Task<ConversationDto> CreateConversationAsync(ConversationDto NewConversation);

        
        Task<IEnumerable<ConversationDto>> GetRecentConversationsAsync();

        
        Task UploadAudioChunkAsync(Guid ConversationId, byte[] AudioData);
    }

    
}
