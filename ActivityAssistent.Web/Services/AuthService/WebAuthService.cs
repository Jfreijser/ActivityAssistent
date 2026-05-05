using System.Net.Http.Json;
using ActivityAssistent.Shared.Dtos.Identity;
using ActivityAssistent.Shared.Interfaces.Identity;

namespace ActivityAssistent.Web.Services.AuthService
{
    public class WebAuthService(HttpClient Http) : IAuthService
    {
        public async Task<UserProfileDto> GetCurrentProfileAsync(CancellationToken Token = default)
        {
            var Response = await Http.GetFromJsonAsync<UserProfileDto>("api/auth/me", cancellationToken: Token);
            return Response!;
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
