using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ActivityAssistent.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        // Deze identifier stuur je mee naar je SalesConversationService
        // zodat de Dataverse-client weet namens wie hij praat.
        protected string CurrentUserEmail => User.Identity?.Name ?? "system@organisatie.nl";

        protected Guid GetCurrentSalesUserId()
        {
            // Hier zou je een mapping kunnen doen van e-mail naar de Dataverse Guid.
            // Voor je PoC kun je dit in het begin simpel houden.
            var UserIdClaim = User.FindFirst("sub")?.Value;
            return Guid.TryParse(UserIdClaim, out var SalesUserId) ? SalesUserId : Guid.Empty;
        }

        // Een helper methode om snel te controleren of de gebruiker een 'Manager' rol heeft
        protected bool IsManager => User.IsInRole("Manager");
    }
}
