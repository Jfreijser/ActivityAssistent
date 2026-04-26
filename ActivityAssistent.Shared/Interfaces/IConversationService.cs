using System;
using System.Collections.Generic;
using System.Text;
using ActivityAssistent.Shared.Dtos.Conversations;

namespace ActivityAssistent.Shared.Interfaces
{
    public interface IConversationService
    {
        Task<ConversationDto> CreateConversationAsync(ConversationDto NewConversation);

        // Haal een lijst op van recente gesprekken voor de salesmedewerker
        Task<IEnumerable<ConversationDto>> GetRecentConversationsAsync();

        // Deze laten we voor nu leeg in de implementatie, maar staat vast in het contract
        Task UploadAudioChunkAsync(Guid ConversationId, byte[] AudioData);
    }

    
}
