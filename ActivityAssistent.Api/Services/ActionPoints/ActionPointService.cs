using ActivityAssistent.Api.Interfaces.ActionPoint;
using ActivityAssistent.Api.Interfaces.Identity;
using ActivityAssistent.Shared.Dtos.ActionPoints;
using ActivityAssistent.Shared.Dtos.Identity;
using ActivityAssistent.Shared.Dtos.Response;

namespace ActivityAssistent.Api.Services.ActionPoints
{
    public class ActionPointService(IActionPointRepository ActionPointRepository, IUserContext UserContext, IUserRepository UserRepository) : IActionPointService
    {
        public async Task<ApiResponse<ActionPointDto>> CreateActionPointAsync(CreateActionPointDto ActionPoint, CancellationToken Token)
        {
            try
            {
                ActionPoint.SubNrId = UserContext.SubNrId ?? Guid.Empty;
                ActionPoint.SalesUserId = await ResolveSalesUserIdAsync(ActionPoint.SalesUserId, Token);

                var createdId = await ActionPointRepository.CreateAsync(ActionPoint, Token);
                if (createdId == Guid.Empty)
                {
                    return Fail(new ActionPointDto(), "Failed to create the action point.");
                }

                var created = await ActionPointRepository.GetByIdAsync(createdId, Token);
                return created is null
                    ? Fail(new ActionPointDto(), "Failed to load the created action point.")
                    : Ok(created);
            }
            catch (Exception ex)
            {
                return Fail(new ActionPointDto(), ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> DeleteActionPointAsync(Guid ActionPointId, CancellationToken Token)
        {
            try
            {
                var result = await ActionPointRepository.DeleteAsync(ActionPointId, Token);
                if (!result)
                {
                    return Fail(false, "Failed to delete the action point.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Fail(false, ex.Message);
            }
        }

        public async Task<ApiResponse<List<ActionPointDto>>> GetActiveActionPointsAsync(string UserId, CancellationToken Token)
        {
            if (!Guid.TryParse(UserId, out var userGuid))
            {
                return Fail(new List<ActionPointDto>(), "Invalid user id.");
            }

            try
            {
                var results = await ActionPointRepository.GetActiveByUserIdAsync(userGuid, Token);
                return Ok(results.ToList());
            }
            catch (Exception ex)
            {
                return Fail(new List<ActionPointDto>(), ex.Message);
            }
        }

        public async Task<ApiResponse<List<ActionPointDto>>> GetByConversationIdAsync(Guid ConversationId, CancellationToken Token)
        {
            try
            {
                var results = await ActionPointRepository.GetByConversationIdAsync(ConversationId, Token);
                return Ok(results.ToList());
            }
            catch (Exception ex)
            {
                return Fail(new List<ActionPointDto>(), ex.Message);
            }
        }

        public async Task<ApiResponse<ActionPointDto>> GetByIdAsync(Guid ActionPointId, CancellationToken Token)
        {
            try
            {
                var result = await ActionPointRepository.GetByIdAsync(ActionPointId, Token);
                return result is null
                    ? Fail(new ActionPointDto(), "Action point not found.")
                    : Ok(result);
            }
            catch (Exception ex)
            {
                return Fail(new ActionPointDto(), ex.Message);
            }
        }

        public async Task<ApiResponse<ActionPointDto>> UpdateActionPointAsync(UpdateActionPointDto UpdatedActionPoint, CancellationToken Token)
        {
            try
            {
                UpdatedActionPoint.SubNrId = UserContext.SubNrId ?? Guid.Empty;
                UpdatedActionPoint.SalesUserId = await ResolveSalesUserIdAsync(UpdatedActionPoint.SalesUserId, Token);

                var result = await ActionPointRepository.UpdateAsync(UpdatedActionPoint, Token);
                if (!result)
                {
                    return Fail(new ActionPointDto(), "Failed to update the action point.");
                }

                var updated = await ActionPointRepository.GetByIdAsync(UpdatedActionPoint.ActionPointId, Token);
                return updated is null
                    ? Fail(new ActionPointDto(), "Failed to load the updated action point.")
                    : Ok(updated);
            }
            catch (Exception ex)
            {
                return Fail(new ActionPointDto(), ex.Message);
            }
        }

        public async Task<ApiResponse<List<UserProfileDto>>> GetDelegationUsersAsync(CancellationToken Token)
        {
            try
            {
                var subNrId = UserContext.SubNrId ?? Guid.Empty;
                if (subNrId == Guid.Empty)
                {
                    return Ok(new List<UserProfileDto>());
                }

                var users = await UserRepository.GetUsersBySubNrIdAsync(subNrId, Token);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return Fail(new List<UserProfileDto>(), ex.Message);
            }
        }

        private async Task<Guid> ResolveSalesUserIdAsync(Guid requestedSalesUserId, CancellationToken token)
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

        private static ApiResponse<T> Ok<T>(T data)
            => new() { IsSuccess = true, Data = data, ErrorMessage = string.Empty };

        private static ApiResponse<T> Fail<T>(T data, string message)
            => new() { IsSuccess = false, Data = data, ErrorMessage = message };
    }
}
