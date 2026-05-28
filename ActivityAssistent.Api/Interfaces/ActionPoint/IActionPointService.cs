using System;
using System.Collections.Generic;
using System.Text;
using ActivityAssistent.Shared.Dtos.ActionPoints;
using ActivityAssistent.Shared.Dtos.Identity;

namespace ActivityAssistent.Api.Interfaces.ActionPoint
{
    public interface IActionPointService
    {
        global::System.Threading.Tasks.Task<ActionPointDto> CreateActionPointAsync(CreateActionPointDto ActionPoint, CancellationToken Token);
        global::System.Threading.Tasks.Task<ActionPointDto> UpdateActionPointAsync(UpdateActionPointDto UpdatedActionPoint, CancellationToken Token);
        global::System.Threading.Tasks.Task<bool> DeleteActionPointAsync(Guid ActionPointId, CancellationToken Token);
        global::System.Threading.Tasks.Task<ActionPointDto> GetByIdAsync(Guid ActionPointId, CancellationToken Token);
        global::System.Threading.Tasks.Task<IEnumerable<ActionPointDto>> GetActiveActionPointsAsync(string UserId, CancellationToken Token);
        global::System.Threading.Tasks.Task<IEnumerable<ActionPointDto>> GetByConversationIdAsync(Guid ConversationId, CancellationToken Token);
        global::System.Threading.Tasks.Task<List<UserProfileDto>> GetDelegationUsersAsync(CancellationToken Token);
    }
}
