using ActivityAssistent.Api.Infrastructure;
using ActivityAssistent.Api.Interfaces.companies;
using ActivityAssistent.Api.Interfaces.Identity;
using ActivityAssistent.Api.Mappings;
using ActivityAssistent.Shared.Dtos.Companies;
using Microsoft.Crm.Sdk.Messages;

namespace ActivityAssistent.Api.Services.Companies
{
    public class CompanyService (ICompanyRepository CompanyRepository, IUserContext UserContext) : ICompanyService
    {
        public async Task<bool> CreateCompanyAsync(CreateCompanyDto CompanyDto, CancellationToken Token)
        {
            var Exist =  await CompanyRepository.GetByNameAsync(CompanyDto.Name, UserContext.CurrentUserId, Token);
            if (Exist.Any())
            {
                throw new Exception("Company with the same name already exists.");
            }

            CompanyDto.OwnerUserId = UserContext.CurrentUserId;
            
            var Id = await CompanyRepository.CreateAsync(CompanyDto, Token);

            return Id != Guid.Empty;
        }

        public Task<bool> DeleteCompanyAsync(Guid CompanyId, CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public async Task<CompanyDto> GetCompanyByIdAsync(Guid CompanyId, CancellationToken Token)
        {
            return await CompanyRepository.GetByIdAsync(CompanyId, Token);
        }

        public  async Task<List<CompanyNames>> GetCustomerNamesAsync(CancellationToken Token)
        {
            try
            {

                var Customers = await CompanyRepository.GetCustomerNamesAsync(UserContext.CurrentUserId, Token);
                return Customers;

            }
            catch (Exception ex )
            {
                Console.WriteLine($"An Error happend while fetching customers: {ex}");
                return new List<CompanyNames>();
            }
        }

        public Task<bool> UpdateCompanyAsync(CompanyDto Company, CancellationToken Token)
        {
            throw new NotImplementedException();
        }
    }
}
