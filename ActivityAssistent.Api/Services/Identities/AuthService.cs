using ActivityAssistent.Shared.Dtos.Identity;
using ActivityAssistent.Shared.Dtos.Identity;
using ActivityAssistent.Shared.Interfaces;
using ActivityAssistent.Shared.Interfaces.Identity;
using Microsoft.Xrm.Sdk; // Voor 'Entity' en 'ColumnSet'
using Microsoft.Xrm.Sdk.Query; // Voor 'ColumnSet'
using Microsoft.PowerPlatform.Dataverse.Client; //
namespace ActivityAssistent.Api.Services
{
    public class AuthService(IUserRepository UserRepository, IUserContext UserContext) : IAuthService
    {
        public async Task<UserProfileDto> GetCurrentProfileAsync(CancellationToken Token = default)
        {
            var CurrentId = UserContext.CurrentUserId;

            if (CurrentId == Guid.Empty)
            {
                throw new UnauthorizedAccessException("Geen actieve gebruiker gevonden.");
            }

            var UserProfile = await UserRepository.GetProfileByIdAsync(CurrentId);

            if (UserProfile == null)
            {
                throw new UnauthorizedAccessException("Geen actieve gebruiker gevonden.");
            }

            return UserProfile;
        }

        public Task<AuthResultDto> LoginAsync(LoginCredentialsDto Credentials, CancellationToken Token = default)
        {
            throw new NotImplementedException();
        }

        public Task LogoutAsync(CancellationToken Token = default)
        {
            throw new NotImplementedException();
        }
    }
}
