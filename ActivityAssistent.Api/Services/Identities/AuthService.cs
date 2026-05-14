using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ActivityAssistent.Api.Infrastructure.Repositories; //
using ActivityAssistent.Shared.Dtos.Identity;
using ActivityAssistent.Shared.Interfaces;
using ActivityAssistent.Shared.Interfaces.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk; // Voor 'Entity' en 'ColumnSet'
using Microsoft.Xrm.Sdk.Query; // Voor 'ColumnSet'

namespace ActivityAssistent.Api.Services
{
    public class AuthService(IUserRepository UserRepository, IUserContext UserContext, IConfiguration configuration) : IAuthService
    {
        public async Task<UserProfileDto> GetCurrentProfileAsync(CancellationToken Token = default)
        {
            var CurrentId = UserContext.CurrentUserId;

            if (CurrentId == Guid.Empty)
            {
                throw new UnauthorizedAccessException("Geen actieve gebruiker gevonden.");
            }

            var UserProfile = await UserRepository.GetProfileByIdAsync(CurrentId);

            if (UserProfile == null)
            {
                throw new UnauthorizedAccessException("Geen actieve gebruiker gevonden.");
            }

            return UserProfile;
        }

        public async Task<AuthResultDto> LoginAsync(LoginCredentialsDto Dto, CancellationToken Token = default)
        {
            var User = await UserRepository.GetUserForLoginByEmailAsync(Dto.Email, Token);
            if (User == null)
            {
                return new AuthResultDto{IsSuccess = false, ErrorMessage = "Ongeldige inloggegevens."};
                
            }

            bool IsPasswordValid = BCrypt.Net.BCrypt.Verify(Dto.Password, User.PasswordHash);
            if (!IsPasswordValid)
            {
                return new AuthResultDto { IsSuccess = false, ErrorMessage = "Email or password incorrect." };
            }

            
            return new AuthResultDto { IsSuccess = true, AccessToken = GenerateJwtToken(User) };

        }

        public Task LogoutAsync(CancellationToken Token = default)
        {
            throw new NotImplementedException();
        }

        private string GenerateJwtToken(UserAuthDto user)
        {
            var jwtSettings = configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()), // Unieke ID uit Dataverse
                new Claim(JwtRegisteredClaimNames.Name, user.FullName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Unieke ID voor dit specifieke token
            };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(8), // Bepaal hoe lang de klant ingelogd blijft
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature) // De versleutelingsmethode
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
