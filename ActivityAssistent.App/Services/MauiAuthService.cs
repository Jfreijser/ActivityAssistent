using ActivityAssistent.App.Auth;
using ActivityAssistent.App.Interfaces.Identity;
using ActivityAssistent.Shared.Dtos.Identity;
using System.Net.Http.Json;

namespace ActivityAssistent.App.Services
{
    public class MauiAuthService(HttpClient Http, CustomAuthenticationStateProvider authStateProvider) : BaseService(Http, authStateProvider), IAuthService
    {
        public async Task<UserProfileDto> GetCurrentProfileAsync(CancellationToken Token = default)
        {
            try
            {
                var Response = await Http.GetFromJsonAsync<UserProfileDto>("api/auth/me", cancellationToken: Token);
                return Response!;
            }
            catch (Exception Ex)
            {

                Console.WriteLine($"Error while logging in: {Ex.Message}");
                return CreateGastProfile();
            }

        }

        public async Task<AuthResultDto> LoginAsync(LoginCredentialsDto Credentials, CancellationToken Token = default)
        {
            try
            {
                var Response = await Http.PostAsJsonAsync("api/auth/login", Credentials, Token);

                // 2. Controleer of de API-oproep überhaupt is geslaagd (status 200-299)
                if (!Response.IsSuccessStatusCode)
                {
                    return new AuthResultDto
                    {
                        IsSuccess = false,
                        ErrorMessage = "An error occurred while communicating with the server."
                    };
                }
                var Result = await Response.Content.ReadFromJsonAsync<AuthResultDto>(cancellationToken: Token);

                // 4. Als het resultaat succesvol is en we hebben een token, geef dit door aan de provider
                if (Result != null && Result.IsSuccess && !string.IsNullOrWhiteSpace(Result.AccessToken))
                {
                    // Hier wordt de token opgeslagen in SecureStorage en de UI bijgewerkt!
                    await authStateProvider.LogInAsync(Result.AccessToken);
                }

                return Result ?? new AuthResultDto { IsSuccess = false, ErrorMessage = "Ongeldige respons van de server." };
            }
            catch (Exception ex )
            {
                Console.WriteLine($"Error while logging in: {ex.Message}");
                throw;
            }
        }

        public Task LogoutAsync(CancellationToken Token = default)
        {
            throw new NotImplementedException();
        }

        private static UserProfileDto CreateGastProfile()
        {
            return new UserProfileDto
            {
                UserId = null,
                FullName = "Gastgebruiker",
                Email = string.Empty,
                Role = string.Empty,
                SubNrId = null
            };
        }
    }
}
