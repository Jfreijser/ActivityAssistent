using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualBasic;
using ActivityAssistent.Shared.Dtos.Conversations;
namespace ActivityAssistent.Api.Interfaces.Conversations
{
    public interface IConversationRepository
    {
        Task<ConversationDto?> GetByIdAsync(Guid ConversationId, CancellationToken Token);

        // 2. Ophalen van alle gesprekken (bijvoorbeeld voor het overzichtsscherm van de salesmedewerker)
        Task<IEnumerable<ConversationDto>> GetAllAsync(Guid OwnerUserId, CancellationToken Token);

        // 3. Ophalen van gesprekken die gekoppeld zijn aan één specifiek bedrijf/klant
        Task<IEnumerable<ConversationDto>> GetByCompanyIdAsync(Guid CompanyId, CancellationToken Token);

        // 4. Het initiële gesprek opslaan (wanneer de audio is opgenomen en de API de verwerking start)
        Task<Guid> CreateAsync(CreateConversationDto Conversation, CancellationToken Token);

        // 5. Het gesprek updaten (cruciaal wanneer de AI-service de samenvatting en actiepunten heeft gegenereerd!)
        Task<bool> UpdateAsync(UpdateConversationDto Conversation, CancellationToken Token);

        // 6. Optioneel: Controleren of een offline gebufferd gesprek al in Dataverse bestaat
        Task<bool> ExistsAsync(Guid ConversationId, CancellationToken Token);
        
    }
}
