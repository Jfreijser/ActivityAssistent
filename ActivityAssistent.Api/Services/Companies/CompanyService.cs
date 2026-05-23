using ActivityAssistent.Api.Infrastructure;
using ActivityAssistent.Api.Interfaces.companies;
using ActivityAssistent.Api.Interfaces.Identity;
using ActivityAssistent.Api.Mappings;
using ActivityAssistent.Shared.Dtos.Companies;

namespace ActivityAssistent.Api.Services.Companies
{
    public class CompanyService (ICompanyRepository CompanyRepository, IUserContext UserContext) : ICompanyService
    {
        public async Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto CompanyDto, CancellationToken Token)
        {
            var Exist =  await CompanyRepository.GetByNameAsync(CompanyDto.Name, Token);
            if (Exist is not null)
            {
                throw new Exception("Company with the same name already exists.");
            }

            CompanyDto.OwnerUserId = UserContext.CurrentUserId;
            
            var Id = await CompanyRepository.CreateAsync(CompanyDto, Token);

            return await CompanyRepository.GetByIdAsync(Id, Token);
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

        public Task<List<CompanyNames>> GetCustomerNamesAsync(CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateCompanyAsync(CompanyDto Company, CancellationToken Token)
        {
            throw new NotImplementedException();
        }
    }
}
