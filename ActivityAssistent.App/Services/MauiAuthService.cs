using System;
using System.Collections.Generic;
using System.Text;
using ActivityAssistent.App.Auth;
using ActivityAssistent.App.Interfaces.Identity;
using ActivityAssistent.Shared.Dtos.Identity;

namespace ActivityAssistent.App.Services
{
    public class MauiAuthService(HttpClient Http, CustomAuthenticationStateProvider authStateProvider) : BaseService(Http, authStateProvider), IAuthService
    {
        public Task<UserProfileDto> GetCurrentProfileAsync(CancellationToken Token = default)
        {
            throw new NotImplementedException();
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
