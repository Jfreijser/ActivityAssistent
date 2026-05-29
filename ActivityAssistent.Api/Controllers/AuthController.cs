using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using ActivityAssistent.Api.Interfaces.Identity;
using ActivityAssistent.Api.Services;
using ActivityAssistent.Shared.Dtos.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ActivityAssistent.Shared.Dtos.Response;
using Microsoft.AspNetCore.Authorization;

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
        [AllowAnonymous]
        public async Task<ActionResult<AuthResultDto>> LoginAsync(LoginCredentialsDto credentials, CancellationToken token = default)
        {
            try
            {
                // 1. Controleer de gegevens en genereer de token via je AuthService
                var Result = await authService.LoginAsync(credentials, token);

                if (!Result.IsSuccess)
                {
                    return Unauthorized(Result);
                }

                // 2. Geen Cookies of SignInAsync meer! We sturen simpelweg de DTO met de JWT token terug.
                return Ok(Result);
            }
            catch (Exception Ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new AuthResultDto
                {
                    IsSuccess = false,
                    ErrorMessage = "Er is een onverwachte fout opgetreden. Probeer het later opnieuw."
                });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }
    }
}
