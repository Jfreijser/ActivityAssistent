using System;
using System.Collections.Generic;
using System.Text;
using ActivityAssistent.Shared.Dtos.Conversations;
using ActivityAssistent.Shared.Dtos.Response;

namespace ActivityAssistent.Api.Interfaces.Conversations
{
    public interface IConversationService
    {
        Task<ApiResponse<ConversationDto>> CreateConversationAsync(CreateConversationDto Conversation, CancellationToken Token);
        Task<ApiResponse<ConversationDto>> UpdateConversationAsync(UpdateConversationDto conversation, CancellationToken Token);
        Task<ApiResponse<bool>> DeleteConversationAsync(Guid ConversationId, CancellationToken Token);
        Task<ApiResponse<ConversationDto>> GetConversationAsync(Guid ConversationId, CancellationToken Token);

        Task<ApiResponse<List<ConversationDto>>> GetRecentConversationsAsync(CancellationToken Token);
        
        Task<ApiResponse<bool>> UploadAudioAsync(Guid ConversationId, byte[] AudioData, CancellationToken Token);
        Task<ApiResponse<List<ConversationDto>>> GetAllAsync(CancellationToken Token);
    }

    
}
