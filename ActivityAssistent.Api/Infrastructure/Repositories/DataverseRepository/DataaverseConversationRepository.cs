using ActivityAssistent.Shared.Dtos.Conversations;
using Microsoft.PowerPlatform.Dataverse.Client;
using Domain = ActivityAssistent.Shared.Dtos.Conversations;

namespace ActivityAssistent.Api.Infrastructure.Repositories.DataverseRepository
{
    public class DataaverseConversationRepository(IOrganizationServiceAsync dataverseClient) 
    {
        public async Task<Guid> CreateAsync(CreateConversationDto Conversation, CancellationToken Token)
        {
            throw new NotImplementedException();
        }


        public Task<bool> ExistsAsync(Guid ConversationId, CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ConversationDto>> GetAllAsync(CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ConversationDto>> GetByCompanyIdAsync(Guid CompanyId, CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public Task<ConversationDto?> GetByIdAsync(Guid ConversationId, CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(ConversationDto Conversation, CancellationToken Token)
        {
            throw new NotImplementedException();
        }
    }
}
