using System.Net.Http.Json;
using ActivityAssistent.Shared.Dtos.Identity;
using ActivityAssistent.Shared.Interfaces.Identity;

namespace ActivityAssistent.WebV2.Client.Services.AuthService
{
    public class WebAuthService(HttpClient Http) : IAuthService
    {
        
        public async Task<UserProfileDto> GetCurrentProfileAsync(CancellationToken Token = default)
        {
            var Response = await Http.GetFromJsonAsync<UserProfileDto>("api/auth/me", cancellationToken: Token);
            return Response!;
        }

        public async Task<AuthResultDto> LoginAsync(LoginCredentialsDto Credentials, CancellationToken Token = default)
        {
            var Response = await Http.PostAsJsonAsync("api/auth/login", Credentials, Token);
            if (Response.IsSuccessStatusCode)
            {
                var result = await Response.Content.ReadFromJsonAsync<AuthResultDto>(cancellationToken: Token);
                return result!;
            }
            else
            {
                var Result = new AuthResultDto()
                {
                    IsSuccess = false
                };
                return Result;
            }
        }

        public Task LogoutAsync(CancellationToken Token = default)
        {
            throw new NotImplementedException();
        }
    }
}
