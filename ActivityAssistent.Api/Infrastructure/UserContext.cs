using ActivityAssistent.Shared.Interfaces.Identity;
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
                // We halen de User uit de huidige HTTP-context
                var User = Accessor.HttpContext?.User;

                // Zoek naar de unieke ID (ObjectIdentifier voor Azure/Dataverse of 'sub')
                var IdClaim = User?.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value
                              ?? User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Als het lukt om de string naar een Guid te vertalen, geven we die terug.
                // Zo niet, dan geven we Guid.Empty terug.
                return Guid.TryParse(IdClaim, out var ValidGuid) ? ValidGuid : Guid.Empty;
            }
        }
    }
}
