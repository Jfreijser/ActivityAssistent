using System;
using System.Collections.Generic;
using System.Text;
using ActivityAssistent.Shared.Dtos.Identity;

namespace ActivityAssistent.Shared.Interfaces.Identity
{
    public interface IUserRepository
    {
        Task<UserProfileDto?> GetProfileByIdAsync(Guid UserId, CancellationToken Token = default);
    }
}
