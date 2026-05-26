using ActivityAssistent.Api.Interfaces.companies;
using ActivityAssistent.Api.Interfaces.Conversations;
using ActivityAssistent.Api.Interfaces.Identity;
using ActivityAssistent.Shared.Dtos.Conversations;
using ActivityAssistent.Shared.Enums;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;

namespace ActivityAssistent.Api.Services.Conversations
{
    public class ConversationService(IConversationRepository ConversationRepository, ICompanyRepository CompanyRepository, IUserContext UserContext) : IConversationService
    {
        public async Task<ConversationDto> CreateConversationAsync(CreateConversationDto Conversation, CancellationToken Token)
        {
            var Company = await CompanyRepository.GetByIdAsync(Conversation.CompanyId, Token);
            if (Company == null || Company.CompanyId == Guid.Empty)
            {
                throw new KeyNotFoundException($"Company with ID {Conversation.CompanyId} was not found.");
            }

            // 2. Maak het gesprek aan in de database
            Conversation.Status = ConversationStatus.Scheduled;
            var CreatedId = await ConversationRepository.CreateAsync(Conversation, Token);
            if (CreatedId == Guid.Empty)
            {
                throw new InvalidOperationException("Failed to create the conversation in the database.");
            }

            return await GetConversationAsync(CreatedId, Token);

        }

        public Task DeleteConversationAsync(Guid conversationId, CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ConversationDto>> GetAllAsync(CancellationToken Token)
        {
            try
            {
                var result = await ConversationRepository.GetAllAsync(UserContext.CurrentUserId, Token);
                return result;
            }
            catch (Exception ex )
            {
                throw new InvalidOperationException("Failed to retrieve conversations from the database.");
            }
          
        }

        public async Task<ConversationDto> GetConversationAsync(Guid ConversationId, CancellationToken Token)
        {
            try
            {
                var result = await ConversationRepository.GetByIdAsync(ConversationId, Token);
                return result;
            }
            catch (Exception)
            {

                return new ConversationDto();
            }
        }

        public Task<IEnumerable<ConversationDto>> GetRecentConversationsAsync(CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public async Task<ConversationDto> UpdateConversationAsync(UpdateConversationDto conversation, CancellationToken Token)
        {
            try
            {
                var result = await ConversationRepository.UpdateAsync(conversation, Token);

                if (result)
                {
                    return await GetConversationAsync(conversation.ConversationId, Token);
                }
                else
                {
                    throw new InvalidOperationException("Failed to update the conversation.");
                }
            }
            catch (Exception ex)
            {

                throw new InvalidOperationException("An error occurred while updating the conversation.", ex);
            }
        }

        public Task UploadAudioAsync(Guid ConversationId, byte[] AudioData, CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        
    }
}
