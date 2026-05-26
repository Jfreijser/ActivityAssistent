using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ActivityAssistent.Api.Infrastructure.Repositories; //
using ActivityAssistent.Api.Interfaces.Identity;
using ActivityAssistent.Shared.Dtos.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace ActivityAssistent.Api.Services
{
    [Authorize]
    public class AuthService(IUserRepository UserRepository, IUserContext UserContext, IConfiguration configuration) : IAuthService
    {
        public async Task<UserProfileDto> GetCurrentProfileAsync(CancellationToken Token = default)
        {
            var CurrentId = UserContext.CurrentUserId;

            if (CurrentId == Guid.Empty)
            {
                return new UserProfileDto
                {
                    UserId = null,
                    FullName = "Gastgebruiker",
                    Email = string.Empty,
                    Role = UserContext.Role,
                    SubNrId = UserContext.SubNrId
                };
            }

            var UserProfile = await UserRepository.GetProfileByIdAsync(CurrentId, Token);

            if (UserProfile == null)
            {
                throw new UnauthorizedAccessException("Geen actieve gebruiker gevonden.");
            }

            return UserProfile;
        }

        [AllowAnonymous]
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

        //public async Task LogoutAsync(CancellationToken Token = default)
        //{
        //    throw new NotImplementedException();
        //}

        private string GenerateJwtToken(UserAuthDto user)
        {
            var JwtSettings = configuration.GetSection("Jwt");
            var Key = Encoding.ASCII.GetBytes(JwtSettings["Key"]!);

            var Claims = new List<Claim>
            {
                // Gebruik ClaimTypes zodat Blazor ze automatisch snapt!
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()), // Voor de User.FindFirst(ClaimTypes.NameIdentifier)
                new Claim(ClaimTypes.Name, user.FullName),                    // Dit vult direct User.Identity.Name in je Blazor menu!
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.RoleName),
                new Claim(ClaimTypes.GroupSid, user.SubNrId?.ToString() ?? string.Empty)
            };

            var TokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(Claims),
                Expires = DateTime.UtcNow.AddHours(8),
                Issuer = JwtSettings["Issuer"],
                Audience = JwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var TokenHandler = new JwtSecurityTokenHandler();
            var Token = TokenHandler.CreateToken(TokenDescriptor);

            return TokenHandler.WriteToken(Token);
        }

        
    }
}
