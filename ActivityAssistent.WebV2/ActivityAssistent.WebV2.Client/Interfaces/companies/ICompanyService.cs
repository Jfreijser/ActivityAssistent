using System;
using System.Collections.Generic;
using System.Text;
using ActivityAssistent.Shared.Dtos.Companies;

namespace ActivityAssistent.WebV2.Client.Interfaces.companies
{
    public interface ICompanyService
    {
        Task<CompanyDto> GetCompanyByIdAsync(Guid CompanyId, CancellationToken Token);
        Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(CancellationToken Token);
        Task UpdateCompanyAsync(CompanyDto Company, CancellationToken Token);
        Task DeleteCompanyAsync(Guid CompanyId, CancellationToken Token);
        Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto Company, CancellationToken Token);
        Task<List<CompanyNames>> GetCustomerAsync(CancellationToken Token);
    }
}
