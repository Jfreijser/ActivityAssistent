using ActivityAssistent.Api.Mappings;
using ActivityAssistent.Shared.Dtos.Companies;
using ActivityAssistent.Shared.Interfaces.companies;

namespace ActivityAssistent.Api.Services.Companies
{
    public class CompanyService (ICompanyRepository CompanyRepository) : ICompanyService
    {
        public async Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto CompanyDto, CancellationToken Token)
        {
            var Exist =  await CompanyRepository.GetByNameAsync(CompanyDto.CompanyName, Token);
            if (Exist is not null)
            {
                throw new Exception("Company with the same name already exists.");
            }

            var NewCompany = CompanyDto.ToEntity();
            var CreatedCompany = await CompanyRepository.CreateAsync(NewCompany, Token);
            return CreatedCompany.ToDto();
        }

        public Task DeleteCompanyAsync(Guid CompanyId, CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public Task<CompanyDto> GetCompanyByIdAsync(Guid CompanyId, CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CustomerDto>> GetCustomerAsync(CancellationToken Token)
        {
            var Customers = await CompanyRepository.GetCustomerAsync(Token);
            List<CustomerDto> CustomerDtos = new List<CustomerDto>();
            foreach (var Customer in Customers)
            {
                CustomerDtos.Add(Customer.ToCustomerDto());
            }
            return CustomerDtos;
        }

        public Task UpdateCompanyAsync(CompanyDto Company, CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        System.Threading.Tasks.Task ICompanyService.DeleteCompanyAsync(Guid CompanyId, CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        System.Threading.Tasks.Task ICompanyService.UpdateCompanyAsync(CompanyDto Company, CancellationToken Token)
        {
            throw new NotImplementedException();
        }
    }
}
