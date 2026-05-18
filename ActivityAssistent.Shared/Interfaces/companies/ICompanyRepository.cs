using System;
using System.Collections.Generic;
using System.Text;
using ActivityAssistent.Shared.Dtos.Companies;

namespace ActivityAssistent.Shared.Interfaces.companies
{
    public interface ICompanyRepository
    {
        Task<Company> GetByIdAsync(Guid CompanyId, CancellationToken Token);
        Task<IEnumerable<Company>> GetAllAsync(CancellationToken Token);
        Task<Company> CreateAsync(Company Company, CancellationToken Token);
        Task UpdateAsync(Company Company, CancellationToken Token);
        Task DeleteAsync(Guid CompanyId, CancellationToken Token);
        Task<string> GetByNameAsync(string Name, CancellationToken Token);
    }
}
