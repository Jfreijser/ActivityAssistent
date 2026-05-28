using System;
using System.Collections.Generic;
using System.Text;
using ActivityAssistent.App.Auth;
using ActivityAssistent.App.Interfaces.ActionPoint;
using ActivityAssistent.Shared.Dtos.ActionPoints;
using ActivityAssistent.Shared.Dtos.Identity;
using static System.Net.WebRequestMethods;

namespace ActivityAssistent.App.Services
{
    public class MauiActionPointService(HttpClient Http, CustomAuthenticationStateProvider authStateProvider) : BaseService(Http, authStateProvider), IActionPointService
    {
        public Task<ActionPointDto> CreateActionPointAsync(CreateActionPointDto ActionPoint, CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteActionPointAsync(Guid ActionPointId, CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public Task<ActionPointDto> GetActionPointByIdAsync(Guid ActionPointId, CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public Task<List<ActionPointDto>> GetActiveActionPointsAsync(string UserId, CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public Task<List<ActionPointDto>> GetByConversationIdAsync(Guid ConversationId, CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserProfileDto>> GetDelegationUsersAsync(CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public Task<ActionPointDto> UpdateActionPointAsync(UpdateActionPointDto UpdatedActionPoint, CancellationToken Token)
        {
            throw new NotImplementedException();
        }
    }
}
