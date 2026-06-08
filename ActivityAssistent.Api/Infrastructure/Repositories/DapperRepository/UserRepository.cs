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
            var command = new CommandDefinition(sql, new { UserId }, cancellationToken: Token);
            using (var conn = connection.CreateConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<UserProfileDto?>(command);
            }
        }

        public async Task<UserAuthDto?> GetUserForLoginByEmailAsync(string Email, CancellationToken Token = default)
        {
            string sql = @"select us.*, Rol.Name as RoleName from Users as us
                           left join Roles as Rol on Rol.RoleId = us.RoleId
                           where Email =@Email";
            var command = new CommandDefinition(sql, new { Email }, cancellationToken: Token);
            using (var conn = connection.CreateConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<UserAuthDto?>(command);
            }
        }

        public async Task<List<UserProfileDto>> GetUsersBySubNrIdAsync(Guid SubNrId, CancellationToken Token = default)
        {
            string sql = "select UserId, FullName, Email, RoleId, SubNrId from Users where SubNrId = @SubNrId";
            var command = new CommandDefinition(sql, new { SubNrId }, cancellationToken: Token);
            using (var conn = connection.CreateConnection())
            {
                var result = await conn.QueryAsync<UserProfileDto>(command);
                return result.ToList();
            }
        }
    }
}
