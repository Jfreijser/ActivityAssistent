using System;
using System.Collections.Generic;
using System.Text;
using ActivityAssistent.Shared.Dtos.ActionPoints;
using ActivityAssistent.Shared.Dtos.Identity;

namespace ActivityAssistent.App.Interfaces.ActionPoint
{
    public interface IActionPointService
    {
        Task<ActionPointDto> CreateActionPointAsync(CreateActionPointDto ActionPoint, CancellationToken Token);
        Task<ActionPointDto> UpdateActionPointAsync(UpdateActionPointDto UpdatedActionPoint, CancellationToken Token);
        Task<bool> DeleteActionPointAsync(Guid ActionPointId, CancellationToken Token);
        Task<ActionPointDto> GetActionPointByIdAsync(Guid ActionPointId, CancellationToken Token);
        Task<List<ActionPointDto>> GetActiveActionPointsAsync(string UserId, CancellationToken Token);
        Task<List<ActionPointDto>> GetByConversationIdAsync(Guid ConversationId, CancellationToken Token);
        Task<List<UserProfileDto>> GetDelegationUsersAsync(CancellationToken Token);
        Task<ActionPointResolutionsDto> ResolveActionPointAsync(CreateActionPointResolutionDto Resolution, CancellationToken Token);

        // 2. Het ophalen van de afsluit-historie voor een specifiek actiepunt (als je dit later in de UI wilt tonen)

        Task<List<ActionPointResolutionsDto>> GetActionPointResolutionsAsync(Guid ActionPointId, CancellationToken Token);
    }
}
