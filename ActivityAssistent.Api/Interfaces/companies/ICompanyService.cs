using ActivityAssistent.Shared.Dtos.Companies;

namespace ActivityAssistent.Api.Interfaces.companies
{
    public interface ICompanyService
    {
        Task<CompanyDto> GetCompanyByIdAsync(Guid CompanyId, CancellationToken Token);
        Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(CancellationToken Token);
        Task <bool> UpdateCompanyAsync(CompanyDto Company, CancellationToken Token);
        Task <bool> DeleteCompanyAsync(Guid CompanyId, CancellationToken Token);
        Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto Company, CancellationToken Token);
        Task<List<CompanyNames>> GetCustomerNamesAsync(CancellationToken Token);
    }
}
