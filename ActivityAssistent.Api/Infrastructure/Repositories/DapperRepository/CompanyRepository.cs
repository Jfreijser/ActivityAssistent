using ActivityAssistent.Api.Interfaces.companies;
using ActivityAssistent.Shared.Dtos.Companies;
using Dapper;

namespace ActivityAssistent.Api.Infrastructure.Repositories.DapperRepository
{
    public class CompanyRepository(IDbConnectionFactory connection) : ICompanyRepository
    {
        public async Task<Guid> CreateAsync(CreateCompanyDto Company, CancellationToken Token)
        {
            string Sql = @"Insert Into Companies(Name, Email, PhoneNumber, CreatedOn, City, Address, OwnerUserId) 
                            OUTPUT INSERTED.CompanyId
                            Values(@Name, @Email, @PhoneNumber, @CreatedOn, @City, @Address, @OwnerUserId)";
            
            var command = new CommandDefinition(Sql, Company, cancellationToken: Token);
            using (var conn = connection.CreateConnection())
            {
                return await conn.QuerySingleAsync<Guid>(command);
                
            }
        }

        public Task<bool> DeleteAsync(Guid CompanyId, CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CompanyDto>> GetAllAsync(Guid OwnerUserId, CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public async Task<CompanyDto> GetByIdAsync(Guid CompanyId, CancellationToken Token)
        {
            string sql = "select * from Companies where CompanyId = @CompanyId";
            var command = new CommandDefinition(sql, new { CompanyId }, cancellationToken: Token);  
            using (var conn = connection.CreateConnection())
            {
                return await conn.QuerySingleAsync<CompanyDto>(command);
            }
        }

        public async Task<List<string>> GetByNameAsync(string Name, Guid OwnerUserId, CancellationToken Token)
        {
            string Sql = "select Name from Companies where Name = @Name and OwnerUserId = @OwnerUserId";
            var command = new CommandDefinition(Sql, new { Name, OwnerUserId }, cancellationToken: Token);
            using (var conn = connection.CreateConnection())
            {
                return (await conn.QueryAsync<string>(command)).ToList();
            }
        }

        public async Task<List<CompanyNames>> GetCompanyNamesAsync(Guid OwnerUserId, CancellationToken Token)
        {
            string sql = "select CompanyId, Name from Companies where OwnerUserId = @OwnerUserId";
            var command = new CommandDefinition(sql, new { OwnerUserId }, cancellationToken: Token);
            using (var conn = connection.CreateConnection())
            {
                return (await conn.QueryAsync<CompanyNames>(command)).ToList();
            }
        }

        public async Task<List<CompanyOverviewDto>> GetCompanyOverviewAsync(Guid SalesUserId, CancellationToken Token)
        {
            string sql = @"SELECT Com.CompanyId, Com.Name AS CompanyName, COUNT(Con.ConversationId) AS TotalConversations, MAX(Con.MeetingDate) AS LastContactDate FROM Companies AS Com
                        LEFT JOIN Conversations AS Con ON Com.CompanyId = Con.CompanyId AND Con.SalesUserId = @SalesUserId
                        GROUP BY 
                            Com.CompanyId, 
                            Com.Name
                        ORDER BY 
                            MAX(Con.MeetingDate) DESC";
            var command = new CommandDefinition(sql, new { SalesUserId}, cancellationToken: Token);
            using (var conn = connection.CreateConnection())
            {
                return (await conn.QueryAsync<CompanyOverviewDto>(command)).ToList();
            }
        }

        public Task<bool> UpdateAsync(UpdateCompanyDto Company, CancellationToken Token)
        {
            throw new NotImplementedException();
        }

    }
}
