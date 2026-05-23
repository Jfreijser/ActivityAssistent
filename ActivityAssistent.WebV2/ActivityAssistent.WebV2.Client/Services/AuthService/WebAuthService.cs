using System.Net.Http.Headers;
using System.Net.Http.Json;
using ActivityAssistent.Shared.Dtos.Identity;
using ActivityAssistent.WebV2.Client.Interfaces.Identity;
using Microsoft.JSInterop;

namespace ActivityAssistent.WebV2.Client.Services.AuthService
{
    public class WebAuthService(HttpClient Http, IJSRuntime JsRuntime) : IAuthService
    {
        private const string TokenKey = "AuthToken";
        public async Task<UserProfileDto> GetCurrentProfileAsync(CancellationToken Token = default)
        {
            try
            {
                //var SavedToken = await JsRuntime.InvokeAsync<string>("localStorage.getItem", TokenKey);

                //if (!string.IsNullOrEmpty(SavedToken))
                //{
                //    Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SavedToken);
                //}
                //else
                //{
                //    Http.DefaultRequestHeaders.Authorization = null;
                //}

                var Response = await Http.GetFromJsonAsync<UserProfileDto>("api/auth/me", cancellationToken: Token);
                return Response!;
            }
            catch (Exception Ex)
            {

                Console.WriteLine($"Fout bij het ophalen van het profiel: {Ex.Message}");
                return CreateGastProfile();
            }
           
        }

        public async Task<AuthResultDto> LoginAsync(LoginCredentialsDto Credentials, CancellationToken Token = default)
        {
            var Response = await Http.PostAsJsonAsync("api/auth/login", Credentials, Token);
            if (Response.IsSuccessStatusCode)
            {
                var result = await Response.Content.ReadFromJsonAsync<AuthResultDto>(cancellationToken: Token);
                if (result != null && result.IsSuccess)
                {
                    await JsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, result.AccessToken);
                }
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

        private UserProfileDto CreateGastProfile()
        {
            return new UserProfileDto
            {
                UserId = null,
                FullName = "Gastgebruiker",
                Email = string.Empty,
                JobTitle = string.Empty
            };
        }
    }
}
