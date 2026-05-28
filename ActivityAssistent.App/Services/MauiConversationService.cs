using System;
using System.Collections.Generic;
using System.Text;
using ActivityAssistent.App.Auth;
using ActivityAssistent.App.Interfaces.Conversations;
using ActivityAssistent.Shared.Dtos.Conversations;

namespace ActivityAssistent.App.Services
{
    public class MauiConversationService(HttpClient Http, CustomAuthenticationStateProvider authStateProvider) : BaseService(Http, authStateProvider), IConversationService
    {
        public Task<ConversationDto> CreateConversationAsync(CreateConversationDto Conversation, CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public Task DeleteConversationAsync(Guid ConversationId, CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public Task<List<ConversationDto>> GetAllAsync(CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public Task<ConversationDto> GetConversationAsync(Guid ConversationId, CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public Task<List<ConversationDto>> GetRecentConversationsAsync(CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public Task<ConversationDto> UpdateConversationAsync(UpdateConversationDto conversation, CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public Task UploadAudioAsync(Guid ConversationId, byte[] AudioData, CancellationToken Token)
        {
            throw new NotImplementedException();
        }
    }
}
