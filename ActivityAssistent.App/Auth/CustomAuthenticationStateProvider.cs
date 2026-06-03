using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
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

                if (JwtToken.ValidTo < DateTime.UtcNow)
                {

                    SecureStorage.Default.Remove(TokenKey);
                    // return a empty claim if token is expired
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                var Claims = JwtToken.Claims.ToList();
                var NameIdentifier = Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                var RoleValue = Claims.FirstOrDefault(c => string.Equals(c.Type, "role", StringComparison.OrdinalIgnoreCase))?.Value;
                var UserName = Claims.FirstOrDefault(c => string.Equals(c.Type, "unique_name", StringComparison.OrdinalIgnoreCase))?.Value;
                var Email = Claims.FirstOrDefault(c => string.Equals(c.Type, "email", StringComparison.OrdinalIgnoreCase))?.Value;
                var GroupSid = Claims.FirstOrDefault(c => string.Equals(c.Type, "groupsid", StringComparison.OrdinalIgnoreCase))?.Value;

                if (!string.IsNullOrWhiteSpace(NameIdentifier)
                    && !Claims.Any(c => string.Equals(c.Type, "nameid", StringComparison.OrdinalIgnoreCase)))
                {
                    Claims.Add(new Claim("nameid", NameIdentifier));
                }

                if (!string.IsNullOrWhiteSpace(RoleValue)
                    && !Claims.Any(c => c.Type == ClaimTypes.Role))
                {
                    Claims.Add(new Claim(ClaimTypes.Role, RoleValue));
                }

                if (!string.IsNullOrWhiteSpace(UserName)
                    && !Claims.Any(c => c.Type == ClaimTypes.Name))
                {
                    Claims.Add(new Claim(ClaimTypes.Name, UserName));
                }

                if (!string.IsNullOrWhiteSpace(Email)
                    && !Claims.Any(c => c.Type == ClaimTypes.Email))
                {
                    Claims.Add(new Claim(ClaimTypes.Email, Email));
                }

                if (!string.IsNullOrWhiteSpace(GroupSid)
                    && !Claims.Any(c => c.Type == ClaimTypes.GroupSid))
                {
                    Claims.Add(new Claim(ClaimTypes.GroupSid, GroupSid));
                }

                var Identity = new ClaimsIdentity(
                                    Claims,
                                    "jwt",
                                    ClaimTypes.Name,
                                    ClaimTypes.Role);

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
