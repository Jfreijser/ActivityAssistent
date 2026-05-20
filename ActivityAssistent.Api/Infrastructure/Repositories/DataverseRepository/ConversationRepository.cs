using ActivityAssistent.Shared.Dtos.Conversations;
using ActivityAssistent.Shared.Interfaces.Conversations;
using Microsoft.PowerPlatform.Dataverse.Client;
using Domain = ActivityAssistent.Shared.Dtos.Conversations;

namespace ActivityAssistent.Api.Infrastructure.Repositories.DataverseRepository
{
    public class ConversationRepository(IOrganizationServiceAsync dataverseClient) : IConversationRepository
    {
        public async Task<Guid> CreateAsync(CreateConversationDto Conversation, CancellationToken Token)
        {
            using var Context = new DataverseContext(dataverseClient);
            var ConversationEntity = new cre7e_SalesConversation
            {
                Id = Guid.NewGuid(),
                cre7e_Title = Conversation.Title
            };
            Context.AddObject(ConversationEntity);
            Context.SaveChanges();
            return ConversationEntity.Id;
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
