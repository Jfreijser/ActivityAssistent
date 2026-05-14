using ActivityAssistent.Shared.Dtos.Identity;
using ActivityAssistent.Shared.Interfaces.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.PowerPlatform.Dataverse.Client;
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

        [HttpPost("login")]
        public async Task<ActionResult<AuthResultDto>> LoginAsync([FromBody]LoginCredentialsDto credentials, CancellationToken token = default)
        {
            try
            { 
                var Result = await authService.LoginAsync(credentials, token);
                if (!Result.IsSuccess)
                {
                    return Unauthorized(Result);
                }
                return Ok(Result);
            }
            catch(Exception Ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new AuthResultDto
                {
                    IsSuccess = false,
                    ErrorMessage = "Er is een onverwachte fout opgetreden. Probeer het later opnieuw."
                });
            }
        }
    }
}
