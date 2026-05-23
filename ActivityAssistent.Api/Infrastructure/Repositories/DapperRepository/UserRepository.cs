using ActivityAssistent.Api.Interfaces.Identity;
using ActivityAssistent.Shared.Dtos.Identity;
using Dapper;
using Microsoft.Crm.Sdk.Messages;


namespace ActivityAssistent.Api.Infrastructure.Repositories.DapperRepository
{
    public class UserRepository(IDbConnectionFactory connection) : IUserRepository
    {
        public async Task<UserProfileDto?> GetProfileByIdAsync(Guid UserId, CancellationToken Token = default)
        {
            string sql = "select * from Users where UserId = @UserId";
            using (var conn = connection.CreateConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<UserProfileDto?>(sql, new { UserId });
            }
        }

        public async Task<UserAuthDto?> GetUserForLoginByEmailAsync(string Email, CancellationToken Token = default)
        {
            string sql = "select * from Users where Email = @Email";
            using (var conn = connection.CreateConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<UserAuthDto?>(sql, new { Email });
            }
        }
    }
}
