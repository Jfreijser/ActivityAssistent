using ActivityAssistent.Shared.Dtos.Conversations;
using ActivityAssistent.Shared.Interfaces.companies;
using ActivityAssistent.Shared.Interfaces.Conversations;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;

namespace ActivityAssistent.Api.Services.Conversations
{
    public class ConversationService(IOrganizationServiceAsync DataverseClient, ICompanyRepository CompanyRepository) : IConversationService
    {
        public async Task<ConversationDto> CreateConversationAsync(CreateConversationDto Conversation, CancellationToken Token)
        {
            //Conversation
            //    var test = await CompanyRepository.GetByIdAsync(Conversation.CompanyId, Token);
            var ent = await CompanyRepository.GetByIdAsync(Conversation.CompanyId, Token);
            if (ent.CompanyId == Guid.Empty)
            {
                throw new InvalidOperationException("Company not found");
            }

            return null;

        }

        public Task DeleteConversationAsync(Guid conversationId, CancellationToken Token)
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

        public async Task<ConversationDto> UpdateConversationAsync(ConversationDto conversation, CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public Task UploadAudioAsync(Guid ConversationId, byte[] AudioData, CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        System.Threading.Tasks.Task IConversationService.DeleteConversationAsync(Guid ConversationId, CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        System.Threading.Tasks.Task IConversationService.UploadAudioAsync(Guid ConversationId, byte[] AudioData, CancellationToken Token)
        {
            throw new NotImplementedException();
        }
    }
}
