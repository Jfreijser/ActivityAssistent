using ActivityAssistent.Shared.Interfaces.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ActivityAssistent.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : BaseApiController
    {
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUserProfile(CancellationToken token = default)
        {
            try
            { 
                var profile = await authService.GetCurrentProfileAsync(token);
                return Ok(profile);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception Ex)
            {
                return BadRequest(Ex.Message);
            }
        }
    }
}
