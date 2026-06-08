using System;
using System.Collections.Generic;
using System.Text;
using ActivityAssistent.Shared.Dtos.Companies;

namespace ActivityAssistent.Api.Interfaces.companies
{
    public interface ICompanyRepository
    {
        Task<CompanyDto> GetByIdAsync(Guid CompanyId, CancellationToken Token);
        Task<IEnumerable<CompanyDto>> GetAllAsync(Guid OwnerUserId, CancellationToken Token);
        Task<Guid> CreateAsync(CreateCompanyDto Company, CancellationToken Token);
        Task <bool>UpdateAsync(UpdateCompanyDto Company, CancellationToken Token);
        Task <bool>DeleteAsync(Guid CompanyId, CancellationToken Token);
        Task<List<string>> GetByNameAsync(string Name, Guid OwnerUserId, CancellationToken Token);
        Task<List<CompanyNames>> GetCompanyNamesAsync(Guid OwnerUserId, CancellationToken Token);
        Task<List<CompanyOverviewDto>> GetCompanyOverviewAsync(Guid OwnerUserId, CancellationToken Token);
    }
}
