using ActivityAssistent.Api.Interfaces.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ActivityAssistent.Api.Infrastructure
{
    public class UserContext(IHttpContextAccessor Accessor) : IUserContext
    {
        public Guid CurrentUserId
        {
            get
            {
                var User = Accessor.HttpContext?.User;

                // Zoek naar de Azure identifier, de standaard .NET identifier, óf direct naar "sub"
                var IdClaim = User?.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value
                              ?? User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? User?.FindFirst("sub")?.Value; // Jouw handmatige 'Sub' claim uit de AuthService

                return Guid.TryParse(IdClaim, out var ValidGuid) ? ValidGuid : Guid.Empty;
            }
        }

        public string Role
        {
            get
            {
                var User = Accessor.HttpContext?.User;
                return User?.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
            }
        }
        

        public Guid? SubNrId
        {
            get 
            {
                var User = Accessor.HttpContext?.User;
                var SubNrClaim = User?.FindFirst(ClaimTypes.GroupSid)?.Value;

                return Guid.TryParse(SubNrClaim, out var ValidGuid) ? ValidGuid : null;
            }
        }
    }
}
