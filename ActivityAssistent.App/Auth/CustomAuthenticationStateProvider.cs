using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Components.Authorization;

namespace ActivityAssistent.App.Auth
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly string TokenKey = "AuthToken";
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // try and get a saved token from secure storage
            var SavedToken = await SecureStorage.Default.GetAsync(TokenKey);

            if (SavedToken == null)
            {
                // return a empty claim if no token is found
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
            try
            {
                var Handler = new JwtSecurityTokenHandler();
                var JwtToken = Handler.ReadJwtToken(SavedToken);

                if (JwtToken.ValidTo < DateTime.Now)
                {

                    SecureStorage.Default.Remove(TokenKey);
                    // return a empty claim if token is expired
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                var Identity = new ClaimsIdentity(JwtToken.Claims, "jwt", "name", "role");
                var User = new ClaimsPrincipal(Identity);
                return new AuthenticationState(User);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing JWT: {ex.Message}");

                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        public async Task LogInAsync(string NewToken)
        {
            // add token to secure storage
            await SecureStorage.Default.SetAsync(TokenKey, NewToken);

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public void LogOut()
        {
            SecureStorage.Default.Remove(TokenKey);

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task<string?> GetTokenAsync()
        {
            return await SecureStorage.Default.GetAsync(TokenKey);
        }
    }
}
