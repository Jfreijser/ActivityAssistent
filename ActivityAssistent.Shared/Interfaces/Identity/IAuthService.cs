using System;
using System.Collections.Generic;
using System.Text;
using ActivityAssistent.Shared.Dtos.Identity;

namespace ActivityAssistent.Shared.Interfaces.Identity
{
    public interface IAuthService
    {
        // Inloggen en een token of bevestiging terugkrijgen
        Task<AuthResultDto> LoginAsync(LoginCredentialsDto Credentials);

        // Profiel en persoonlijke instellingen ophalen
        Task<UserProfileDto> GetCurrentProfileAsync();

        // Uitloggen
        Task LogoutAsync();
    }
}
