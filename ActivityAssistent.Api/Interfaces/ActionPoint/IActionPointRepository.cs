using ActivityAssistent.Shared.Dtos.ActionPoints;

namespace ActivityAssistent.Api.Interfaces.ActionPoint
{
    public interface IActionPointRepository
    {
        Task<ActionPointDto?> GetByIdAsync(Guid ActionPointId, CancellationToken Token);
        Task<IEnumerable<ActionPointDto>> GetByConversationIdAsync(Guid ConversationId, CancellationToken Token);
        Task<IEnumerable<ActionPointDto>> GetActiveByUserIdAsync(Guid SalesUserId, CancellationToken Token);
        Task<Guid> CreateAsync(CreateActionPointDto ActionPoint, CancellationToken Token);
        Task<bool> UpdateAsync(UpdateActionPointDto ActionPoint, CancellationToken Token);
        Task<bool> DeleteAsync(Guid ActionPointId, CancellationToken Token);
    }
}
