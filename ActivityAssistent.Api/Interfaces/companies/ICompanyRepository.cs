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
        Task<string> GetByNameAsync(string Name, CancellationToken Token);
        Task<List<CompanyNames>> GetCustomerNamesAsync(Guid OwnerUserId, CancellationToken Token);
    }
}
