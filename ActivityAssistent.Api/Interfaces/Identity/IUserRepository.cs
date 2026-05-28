using System;
using System.Collections.Generic;
using System.Text;
using ActivityAssistent.Shared.Dtos.Identity;

namespace ActivityAssistent.Api.Interfaces.Identity
{
    public interface IUserRepository
    {
        Task<UserProfileDto?> GetProfileByIdAsync(Guid UserId, CancellationToken Token = default);
        Task<UserAuthDto?> GetUserForLoginByEmailAsync(string Email, CancellationToken Token = default);
        Task<List<UserProfileDto>> GetUsersBySubNrIdAsync(Guid SubNrId, CancellationToken Token = default);
    }

}
