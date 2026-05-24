using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Http;
namespace ActivityAssistent.WebV2
{
    public class CustomAuthenticationStateProvider(IHttpContextAccessor _httpContextAccessor) : AuthenticationStateProvider
    {
        
        private readonly string TokenKey = "AuthToken";
        private string? token;

        

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var savedToken = _httpContextAccessor.HttpContext?.Request.Cookies[TokenKey];

            if (string.IsNullOrWhiteSpace(savedToken))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            try
            {
                var Handler = new JwtSecurityTokenHandler();
                var JwtToken = Handler.ReadJwtToken(savedToken);

                // Controleer of token verlopen is
                if (JwtToken.ValidTo < DateTime.UtcNow)
                {
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                var Identity = new ClaimsIdentity(JwtToken.Claims, "jwt", "name", "role");
                var User = new ClaimsPrincipal(Identity);
                return new AuthenticationState(User);
            }
            catch
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        // Deze methode roep je aan vanuit je LoginAsync functie nadat de token is opgeslagen
        public void NotifyUserLoggedIn()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        // Deze roep je aan bij uitloggen
        public void NotifyUserLogout()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public string? GetToken()
        {
            return _httpContextAccessor.HttpContext?.Request.Cookies[TokenKey];
        }

        public void SetToken(string? newToken)
        {
            token = newToken; // Update je "notitieblokje" direct
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync()); // Refresh de UI
        }
    }
}