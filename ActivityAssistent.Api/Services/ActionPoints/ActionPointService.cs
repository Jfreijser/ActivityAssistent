using ActivityAssistent.Api.Interfaces.ActionPoint;
using ActivityAssistent.Api.Interfaces.Identity;
using ActivityAssistent.Shared.Dtos.ActionPoints;
using ActivityAssistent.Shared.Dtos.Identity;

namespace ActivityAssistent.Api.Services.ActionPoints
{
    public class ActionPointService(IActionPointRepository ActionPointRepository, IUserContext UserContext, IUserRepository UserRepository) : IActionPointService
    {
        public async global::System.Threading.Tasks.Task<ActionPointDto> CreateActionPointAsync(CreateActionPointDto ActionPoint, CancellationToken Token)
        {
            ActionPoint.SubNrId = UserContext.SubNrId ?? Guid.Empty;
            ActionPoint.SalesUserId = await ResolveSalesUserIdAsync(ActionPoint.SalesUserId, Token);

            var createdId = await ActionPointRepository.CreateAsync(ActionPoint, Token);
            if (createdId == Guid.Empty)
            {
                throw new InvalidOperationException("Failed to create the action point.");
            }

            var created = await ActionPointRepository.GetByIdAsync(createdId, Token);
            return created ?? new ActionPointDto();
        }

        public async global::System.Threading.Tasks.Task<bool> DeleteActionPointAsync(Guid ActionPointId, CancellationToken Token)
        {
            var result = await ActionPointRepository.DeleteAsync(ActionPointId, Token);
            if (!result)
            {
                throw new InvalidOperationException("Failed to delete the action point.");
            }

            return true;
        }

        public async global::System.Threading.Tasks.Task<IEnumerable<ActionPointDto>> GetActiveActionPointsAsync(string UserId, CancellationToken Token)
        {
            if (!Guid.TryParse(UserId, out var userGuid))
            {
                return new List<ActionPointDto>();
            }

            return await ActionPointRepository.GetActiveByUserIdAsync(userGuid, Token);
        }

        public async global::System.Threading.Tasks.Task<IEnumerable<ActionPointDto>> GetByConversationIdAsync(Guid ConversationId, CancellationToken Token)
        {
            return await ActionPointRepository.GetByConversationIdAsync(ConversationId, Token);
        }

        public async global::System.Threading.Tasks.Task<ActionPointDto> GetByIdAsync(Guid ActionPointId, CancellationToken Token)
        {
            var result = await ActionPointRepository.GetByIdAsync(ActionPointId, Token);
            return result ?? new ActionPointDto();
        }

        public async global::System.Threading.Tasks.Task<ActionPointDto> UpdateActionPointAsync(UpdateActionPointDto UpdatedActionPoint, CancellationToken Token)
        {
            UpdatedActionPoint.SubNrId = UserContext.SubNrId ?? Guid.Empty;
            UpdatedActionPoint.SalesUserId = await ResolveSalesUserIdAsync(UpdatedActionPoint.SalesUserId, Token);

            var result = await ActionPointRepository.UpdateAsync(UpdatedActionPoint, Token);
            if (!result)
            {
                throw new InvalidOperationException("Failed to update the action point.");
            }

            var updated = await ActionPointRepository.GetByIdAsync(UpdatedActionPoint.ActionPointId, Token);
            return updated ?? new ActionPointDto();
        }

        public async global::System.Threading.Tasks.Task<List<UserProfileDto>> GetDelegationUsersAsync(CancellationToken Token)
        {
            var subNrId = UserContext.SubNrId ?? Guid.Empty;
            if (subNrId == Guid.Empty)
            {
                return new List<UserProfileDto>();
            }

            return await UserRepository.GetUsersBySubNrIdAsync(subNrId, Token);
        }

        private async global::System.Threading.Tasks.Task<Guid> ResolveSalesUserIdAsync(Guid requestedSalesUserId, CancellationToken token)
        {
            var currentUserId = UserContext.CurrentUserId;
            var subNrId = UserContext.SubNrId ?? Guid.Empty;

            if (requestedSalesUserId == Guid.Empty || requestedSalesUserId == currentUserId)
            {
                return currentUserId;
            }

            if (subNrId == Guid.Empty)
            {
                throw new InvalidOperationException("Delegation is not available without a SubNr context.");
            }

            var users = await UserRepository.GetUsersBySubNrIdAsync(subNrId, token);
            if (!users.Any(x => x.UserId == requestedSalesUserId))
            {
                throw new InvalidOperationException("Selected sales user is not in the same SubNr.");
            }

            return requestedSalesUserId;
        }
    }
}
