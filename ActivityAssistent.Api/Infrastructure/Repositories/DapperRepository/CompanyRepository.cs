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

            using (var conn = connection.CreateConnection())
            {
                return await conn.QuerySingleAsync<Guid>(Sql, Company);
                
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
            
            using (var conn = connection.CreateConnection())
            {
                return await conn.QuerySingleAsync<CompanyDto>(sql, new { CompanyId });
            }
        }

        public async Task<List<string>> GetByNameAsync(string Name, Guid OwnerUserId, CancellationToken Token)
        {
            string Sql = "select Name from Companies where Name = @Name and OwnerUserId = @OwnerUserId";

            using (var conn = connection.CreateConnection())
            {
                return (await conn.QueryAsync<string>(Sql, new { Name, OwnerUserId })).ToList();
            }
        }

        public async Task<List<CompanyNames>> GetCustomerNamesAsync(Guid OwnerUserId, CancellationToken Token)
        {
            string sql = "select CompanyId, Name from Companies where OwnerUserId = @OwnerUserId";

            using (var conn = connection.CreateConnection())
            {
                return (await conn.QueryAsync<CompanyNames>(sql, new { OwnerUserId = OwnerUserId })).ToList();
            }
        }

        public Task<bool> UpdateAsync(UpdateCompanyDto Company, CancellationToken Token)
        {
            throw new NotImplementedException();
        }
    }
}
