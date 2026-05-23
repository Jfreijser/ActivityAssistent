using System;
using System.Collections.Generic;
using System.Text;
using ActivityAssistent.Shared.Dtos.Identity;

namespace ActivityAssistent.Api.Interfaces.Identity
{
    public interface IAuthService
    {
        // Inloggen en een token of bevestiging terugkrijgen
        Task<AuthResultDto> LoginAsync(LoginCredentialsDto Credentials, CancellationToken Token = default);

        // Profiel en persoonlijke instellingen ophalen
        Task<UserProfileDto> GetCurrentProfileAsync(CancellationToken Token = default);

        // Uitloggen
        Task LogoutAsync(CancellationToken Token = default);
    }
}
