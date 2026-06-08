using System;
using System.Collections.Generic;
using System.Text;
using ActivityAssistent.Shared.Dtos.ActionPoints;
using ActivityAssistent.Shared.Dtos.Identity;
using ActivityAssistent.Shared.Dtos.Response;

namespace ActivityAssistent.Api.Interfaces.ActionPoint
{
    public interface IActionPointService
    {
        Task<ApiResponse<ActionPointDto>> CreateActionPointAsync(CreateActionPointDto ActionPoint, CancellationToken Token);
        Task<ApiResponse<ActionPointDto>> UpdateActionPointAsync(UpdateActionPointDto UpdatedActionPoint, CancellationToken Token);
        Task<ApiResponse<bool>> DeleteActionPointAsync(Guid ActionPointId, CancellationToken Token);
        Task<ApiResponse<ActionPointDto>> GetByIdAsync(Guid ActionPointId, CancellationToken Token);
        Task<ApiResponse<List<ActionPointDto>>> GetActiveActionPointsAsync(string UserId, CancellationToken Token);
        Task<ApiResponse<List<ActionPointDto>>> GetByConversationIdAsync(Guid ConversationId, CancellationToken Token);
        Task<ApiResponse<List<UserProfileDto>>> GetDelegationUsersAsync(CancellationToken Token);
    }
}
